using System.Collections.Generic;
using MachineLearningGames.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFramework.Extensions
{
    [TestClass]
    public class CircularListExtensionsTest
    {
        [DataTestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 2)]
        [DataRow(2, 0)]
        public void NextOrFirst_should_return_correct_item(int currentIndex, int expectedIndex)
        {
            // Arrange
            var items = new List<int>
            {
                1, 2, 3
            };

            var expectedItem = items[expectedIndex];

            // Act
            var actualItem = items.NextOrFirst(currentIndex);

            // Assert
            Assert.AreEqual(expectedItem, actualItem);
        }

        [DataTestMethod]
        [DataRow(0, 2)]
        [DataRow(1, 0)]
        [DataRow(2, 1)]
        public void PreviousOrLast_should_return_correct_item(int currentIndex, int expectedIndex)
        {
            // Arrange
            var items = new List<int>
            {
                1, 2, 3
            };

            var expectedItem = items[expectedIndex];

            // Act
            var actualItem = items.PreviousOrLast(currentIndex);

            // Assert
            Assert.AreEqual(expectedItem, actualItem);
        }
    }
}
