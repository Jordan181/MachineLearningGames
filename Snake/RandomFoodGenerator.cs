using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Snake
{
    internal sealed class RandomFoodGenerator
    {
        private readonly GridDescription gridDescription;
        private readonly Random randomGenerator;
        private readonly int squaresCount;

        public RandomFoodGenerator(GridDescription gridDescription, Random randomGenerator)
        {
            this.gridDescription = gridDescription;
            this.randomGenerator = randomGenerator;
            squaresCount = gridDescription.RowCount * gridDescription.ColumnCount;
        }

        public IReadOnlyList<Vector2> GetEmptyGridSquares(IReadOnlyList<Vector2> snakePositions, Vector2 currentFoodPosition)
        {
            var emptySquares = new List<Vector2>(squaresCount);

            for (var x = 0; x < gridDescription.ColumnCount; x++)
            {
                for (var y = 0; y < gridDescription.RowCount; y++)
                {
                    var currentPosition = new Vector2(x, y);
                    if (snakePositions.Contains(currentPosition) || currentFoodPosition == currentPosition)
                        continue;

                    emptySquares.Add(new Vector2(x, y));
                }
            }

            return emptySquares;
        }

        public Vector2 GenerateFoodPosition(IReadOnlyList<Vector2> emptySquares) => emptySquares[randomGenerator.Next(emptySquares.Count)];
    }
}
