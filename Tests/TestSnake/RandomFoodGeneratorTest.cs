using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Snake;

namespace TestSnake
{
    [TestClass]
    public class RandomFoodGeneratorTest
    {
        [TestMethod]
        public void GetEmptyGridSquares_should_return_all_squares_not_containing_snake_or_last_food()
        {
            // Arrange
            var gridDescription = new GridDescription(2, 3, 3);
            var randomGenerator = new Random();
            var randomFoodGenerator = new RandomFoodGenerator(gridDescription, randomGenerator);

            var snakePositions = new List<Vector2>
            {
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 2)
            };

            var foodPosition = new Vector2(2, 1);

            var expectedEmpty = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, 2),
                new Vector2(2, 0),
                new Vector2(2, 2)
            };

            // Act
            var actualEmpty = randomFoodGenerator.GetEmptyGridSquares(snakePositions, foodPosition);

            // Assert
            Assert.IsTrue(expectedEmpty.SequenceEqual(actualEmpty));
        }
    }
}
