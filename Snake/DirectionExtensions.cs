using System;
using Microsoft.Xna.Framework;

namespace Snake
{
    internal static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.None:
                    return Direction.None;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static Vector2 ToTranslation(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Vector2.UnitX;
                case Direction.Left:
                    return Vector2.UnitX * -1;
                case Direction.Up:
                    return Vector2.UnitY * -1;
                case Direction.Down:
                    return Vector2.UnitY;
                case Direction.None:
                    return Vector2.Zero;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
