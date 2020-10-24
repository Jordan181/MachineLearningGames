using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Snake
{
    public class Snake
    {
        public Direction CurrentDirection { get; private set; }
        public List<Vector2> Positions { get; }

        private Snake(List<Vector2> parts)
        {
            Positions = parts;
            CurrentDirection = Direction.None;
        }

        public static Snake Create(int initialX, int initialY, int initialLength)
        {
            var points = new List<Vector2>();

            for (var i = 0; i < initialLength; i++)
                points.Add(new Vector2(initialX, initialY));

            return new Snake(points);
        }

        public void Update(Direction newDirection)
        {
            if (newDirection != Direction.None)
            {
                var (dx, dy) = newDirection.ToTranslation();

                var oldPosition = Positions[0];
                var newX = oldPosition.X + dx;
                var newY = oldPosition.Y + dy;
                Positions[0] = new Vector2(newX, newY);

                for (var i = 1; i < Positions.Count; i++)
                {
                    var newOldPosition = Positions[i];
                    Positions[i] = oldPosition;
                    oldPosition = newOldPosition;
                }

                CurrentDirection = newDirection;
            }
        }

        public void AddBodyPart()
        {
            Positions.Add(Positions[^1]);
        }

        public bool HasHitSelf() 
            => CurrentDirection != Direction.None && Positions
                .Skip(1)
                .Any(body => body.Equals(Positions[0]));

        public bool IsOutOfBounds(GridDescription gridDescription)
        {
            var (headX, headY) = Positions[0];

            return headX < 0 ||
                   headX >= gridDescription.ColumnCount ||
                   headY < 0 ||
                   headY >= gridDescription.RowCount;
        }

        public bool HasEatenFood(Vector2 foodPosition) => Positions[0].Equals(foodPosition);
    }
}
