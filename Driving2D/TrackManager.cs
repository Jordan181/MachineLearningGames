using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Driving2D
{
    public sealed class TrackManager
    {
        private readonly Color[] colorMap;
        private readonly int mapWidth;
        private readonly IReadOnlyList<Vector2> checkPoints;

        internal TrackManager(Color[] colorMap, int mapWidth, IReadOnlyList<Vector2> checkPoints)
        {
            this.colorMap = colorMap;
            this.mapWidth = mapWidth;
            this.checkPoints = checkPoints;
        }

        public int NumberOfCheckpoints => checkPoints.Count;

        public static TrackManager Create(Texture2D trackBounds, IReadOnlyList<float[]> checkPoints)
        {
            var colorMap = trackBounds.GetPixelColorData();
            var checkPointVectors = checkPoints.Select(point => new Vector2(point[0], point[1])).ToList();
            return new TrackManager(colorMap, trackBounds.Width, checkPointVectors);
        }

        public (Vector2 position, Vector2 orientation) GetCarStartingPosition()
        {
            var position = checkPoints[0];
            var orientation = Vector2.Normalize(checkPoints[1] - checkPoints[0]);
            return (position, orientation);
        }

        public bool IsOnTrack(Vector2 point) => GetPixelColor(point) != Color.Black;

        public bool HasPassedTargetCheckpoint(Car car)
        {
            var targetCheckpoint = checkPoints[car.TargetCheckPoint];
            var nextCheckpoint = checkPoints.NextOrFirst(car.TargetCheckPoint);

            var distanceSquaredBetweenCps = Vector2.DistanceSquared(targetCheckpoint, nextCheckpoint);
            var vectorToTarget = targetCheckpoint - car.Position;
            var distanceSquaredToTarget = vectorToTarget.LengthSquared();

            if (distanceSquaredToTarget > distanceSquaredBetweenCps)
                return false;
            
            var currentTrackDirection = nextCheckpoint - targetCheckpoint;
            return Vector2.Dot(vectorToTarget, currentTrackDirection) <= 0;
        }

        private Color GetPixelColor(Vector2 point)
        {
            var index = (int)point.X + (int)point.Y * mapWidth;
            return index > 0 && index < colorMap.Length
                ? colorMap[index]
                : Color.Black;
        }
    }
}
