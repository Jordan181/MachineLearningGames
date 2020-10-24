using FlappyBird.Control;
using FlappyBird.Controllers;

namespace FlappyBird
{
    internal sealed class Bird
    {
        private readonly IFlappyBirdController controller;
        private readonly float size;
        private readonly float gravity;
        private readonly float yLimit;
        private readonly float leftPoint;
        private readonly float rightPoint;
        private float BottomPoint => Y + size;
        private float TopPoint => Y;

        private float speed;
        private bool canFlap;

        public int Id => controller.Id;
        public int Score { get; set; }
        public bool IsAlive { get; set; }
        public float Y { get; private set; }
        
        internal Bird(IFlappyBirdController controller, float x, float size, float gravity, float yLimit)
        {
            this.controller = controller;
            this.size = size;
            this.gravity = gravity;
            this.yLimit = yLimit;
            leftPoint = x;
            rightPoint = x + size;

            Y = yLimit / 2;
            IsAlive = true;
            canFlap = true;
        }
        
        public void Update(Pipe nextPipe)
        {
            var controllerInputs = new FlappyBirdControllerInputs(Y, speed, canFlap, nextPipe?.LeftSide - leftPoint ?? 0f, nextPipe?.CentreOfGap - Y ?? 0f);
            if (controller.Flap(controllerInputs))
            {
                if (canFlap)
                {
                    speed = -5f;
                    canFlap = false;
                }
            }
            else if (!canFlap)
                canFlap = true;

            speed += gravity;
            Y += speed;

            if (HasPassed(nextPipe))
                Score++;

            if (HasHit(nextPipe) || IsOutOfBounds())
                IsAlive = false;
        }

        private bool HasPassed(Pipe pipe) => pipe != null && leftPoint >= pipe.RightSide;

        private bool HasHit(Pipe pipe)
        {
            if (pipe == null || rightPoint < pipe.LeftSide || HasPassed(pipe))
                return false;

            return TopPoint <= pipe.TopFace || BottomPoint >= pipe.BottomFace;
        }

        private bool IsOutOfBounds() => TopPoint > yLimit;
    }
}
