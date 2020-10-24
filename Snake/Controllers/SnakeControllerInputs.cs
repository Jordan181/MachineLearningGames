using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Snake.Controllers
{
    internal sealed class SnakeControllerInputs
    {
        public IReadOnlyList<Vector2> SnakePositions { get; }
        public Vector2 FoodPosition { get; }
        public GridDescription GridDescription { get; }

        public SnakeControllerInputs(IReadOnlyList<Vector2> snakePositions, Vector2 foodPosition, GridDescription gridDescription)
        {
            SnakePositions = snakePositions;
            FoodPosition = foodPosition;
            GridDescription = gridDescription;
        }
    }
}