using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal sealed class Pipe
    {
        public Rectangle Top { get; private set; }
        public Rectangle Bottom { get; private set; }

        public float RightSide => Top.Right;
        public float LeftSide => Top.Left;
        public float TopFace => Top.Bottom;
        public float BottomFace => Bottom.Top;
        public float CentreOfGap => 0.5f * (BottomFace + TopFace);

        public Pipe(Rectangle top, Rectangle bottom)
        {
            Top = top;
            Bottom = bottom;
        }

        public void Update(int speed)
        {
            Top = Update(Top, speed);
            Bottom = Update(Bottom, speed);
        }

        public bool IsOffScreen() => Top.Right <= 0;

        private static Rectangle Update(Rectangle rectangle, int speed)
        {
            var newX = rectangle.X - speed;
            return new Rectangle(newX, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}
    