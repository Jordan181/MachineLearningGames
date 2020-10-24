using FlappyBird.Controllers;

namespace FlappyBird.Control
{
    internal interface IFlappyBirdController
    {
        int Id { get; }

        bool Flap(FlappyBirdControllerInputs controllerInputs);
    }
}