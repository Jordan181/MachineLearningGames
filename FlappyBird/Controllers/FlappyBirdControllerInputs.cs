namespace FlappyBird.Controllers
{
    internal sealed class FlappyBirdControllerInputs
    {
        public float BirdY { get; }
        public float BirdSpeed { get; }
        public bool CanFlap { get; }
        public float PipeDistanceX { get; }
        public float PipeGapDistanceY { get; }

        public FlappyBirdControllerInputs(float birdY, float birdSpeed, bool canFlap, float pipeDistanceX, float pipeGapDistanceY)
        {
            BirdY = birdY;
            BirdSpeed = birdSpeed;
            CanFlap = canFlap;
            PipeDistanceX = pipeDistanceX;
            PipeGapDistanceY = pipeGapDistanceY;
        }
    }
}
