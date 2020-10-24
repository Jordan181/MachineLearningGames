using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using Microsoft.Xna.Framework;

namespace Snake.Controllers
{
    internal sealed class AiControllerInputBuilder
    {
        private static readonly IReadOnlyDictionary<Direction, Func<Vector2, GridDescription, bool>> InBoundsConditions = new Dictionary<Direction, Func<Vector2, GridDescription, bool>>
        {
            {Direction.Right, (vector, g) => vector.X < g.ColumnCount},
            {Direction.Left, (vector, g) => vector.X >= 0},
            {Direction.Up, (vector, g) => vector.Y >= 0},
            {Direction.Down, (vector, g) => vector.Y < g.RowCount}
        };

        private readonly SnakeControllerInputs controllerInputs;
        private readonly List<float> inputs;

        public AiControllerInputBuilder(SnakeControllerInputs controllerInputs)
        {
            this.controllerInputs = controllerInputs;
            inputs = new List<float>();
        }

        public AiControllerInputBuilder LookRight() => LookInDirection(Direction.Right);
        public AiControllerInputBuilder LookLeft() => LookInDirection(Direction.Left);
        public AiControllerInputBuilder LookUp() => LookInDirection(Direction.Up);
        public AiControllerInputBuilder LookDown() => LookInDirection(Direction.Down);

        public AiControllerInputBuilder LookForFood()
        {
            var vectorToFood = Vector2.Normalize(controllerInputs.FoodPosition - controllerInputs.SnakePositions[0]);
            var distance = Vector2.Distance(controllerInputs.FoodPosition, controllerInputs.SnakePositions[0]);

            inputs.Add(vectorToFood.X);
            inputs.Add(vectorToFood.Y);
            inputs.Add(ActivationFunctions.ExpMinusModX(distance));

            return this;
        }

        public Vector<float> BuildInputs() => Vector.Build.DenseOfEnumerable(inputs);

        private AiControllerInputBuilder LookInDirection(Direction direction)
        {
            var movement = direction.ToTranslation();
            var currentIndex = controllerInputs.SnakePositions[0] + movement;
            var snakeBodyIndices = controllerInputs.SnakePositions.Skip(1);
            var inBounds = InBoundsConditions[direction];
            
            var distance = 0f;
            while (inBounds(currentIndex, controllerInputs.GridDescription))
            {
                if (snakeBodyIndices.Contains(currentIndex))
                {
                    break;
                }

                currentIndex += movement;
                distance++;
            }

            inputs.Add(ActivationFunctions.ExpMinusModX(distance));

            return this;
        }
    }
}
