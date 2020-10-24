using System;
using System.Collections.Generic;
using Driving2D;
using Driving2D.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Moq;

namespace TestDriving2D
{
    [TestClass]
    public class TrackManagerTest
    {
        [DataTestMethod]
        [DataRow(100, 120, 0, -1, false)]
        [DataRow(90, 120, 0, -1, false)]
        [DataRow(110, 120, 0, -1, false)]
        [DataRow(100, 101, 0, -1, false)]
        [DataRow(100, 100, 0, -1, true)]
        [DataRow(100, 80, 0, -1, true)]
        [DataRow(80, 90, 0, -1, true)]
        [DataRow(120, 90, 0, -1, true)]
        [DataRow(100, 69, 0, -1, false)]
        public void Should_correctly_identify_if_passed_next_checkpoint_when_track_is_straight_up(
            float carX,
            float carY,
            float carDirectionX,
            float carDirectionY,
            bool expectedHasPassed)
        {
            // Arrange
            var colorMap = Array.Empty<Color>();
            const int mapWidth = 1000;

            var checkPoints = new List<Vector2>
            {
                new Vector2(100, 100),
                new Vector2(100, 70)
            };

            var trackManager = new TrackManager(colorMap, mapWidth, checkPoints);

            var carPosition = new Vector2(carX, carY);
            var carDirection = new Vector2(carDirectionX, carDirectionY);
            var mockController = new Mock<IDriving2DController>();
            var car = new Car(carPosition, carDirection, mockController.Object, 10, 10);

            // Act
            var hasPassed = trackManager.HasPassedTargetCheckpoint(car);
            
            // Assert
            Assert.AreEqual(expectedHasPassed, hasPassed);
        }
    }
}
