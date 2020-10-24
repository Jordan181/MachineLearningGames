using FlappyBird.Control;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird.Controllers
{
    internal sealed class FlappyBirdKeyboardController : IFlappyBirdController
    {
        private readonly Keys flapKey;

        public int Id { get; }

        public FlappyBirdKeyboardController(int id, Keys flapKey = Keys.Space)
        {
            Id = id;
            this.flapKey = flapKey;
        }

        public bool Flap(FlappyBirdControllerInputs controllerInputs) => Keyboard.GetState().IsKeyDown(flapKey);
    }
}
