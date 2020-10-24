using MachineLearningGames.Framework.Ai;
using MathNet.Numerics.LinearAlgebra.Single;

namespace Snake.Controllers
{
    internal sealed class SnakeAiController : ISnakeController
    {
        private readonly NeuralNetwork neuralNetwork;

        public int Id { get; }

        public SnakeAiController(int id, NeuralNetwork neuralNetwork)
        {
            Id = id;
            this.neuralNetwork = neuralNetwork;
        }
        
        public Direction ChooseDirection(SnakeControllerInputs controllerInputs)
        {
            var inputs = new AiControllerInputBuilder(controllerInputs)
                .LookRight()
                .LookLeft()
                .LookUp()
                .LookDown()
                .LookForFood()
                .BuildInputs();

            var outputs = neuralNetwork.GetOutputs(Vector.Build.DenseOfEnumerable(inputs));

            return (Direction) outputs.MaximumIndex();
        }
    }
}