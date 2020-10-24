using Microsoft.Xna.Framework.Input;

namespace Snake.Controllers
{
    internal sealed class SnakeKeyboardController : ISnakeController
    {
        public int Id { get; }

        public SnakeKeyboardController(int id)
        {
            Id = id;
        }

        public Direction ChooseDirection(SnakeControllerInputs controllerInputs)
        {
            var kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
                return Direction.Up;
            if (kb.IsKeyDown(Keys.A))
                return Direction.Left;
            if (kb.IsKeyDown(Keys.S))
                return Direction.Down;
            if (kb.IsKeyDown(Keys.D))
                return Direction.Right;

            return Direction.None;
        }
    }
}
