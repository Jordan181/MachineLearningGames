using Microsoft.Xna.Framework.Input;

namespace Driving2D.Controllers
{
    public class Driving2DKeyboardController : IDriving2DController
    {
        public int Id { get; }

        public Driving2DKeyboardController(int id)
        {
            Id = id;
        }

        public (SpeedAction speedAction, TurningAction turningAction) GetDrivingActions(Car car, TrackManager trackManager)
        {
            var kb = Keyboard.GetState();

            SpeedAction speedAction;
            TurningAction turningAction;

            if (kb.IsKeyDown(Keys.W))
                speedAction = SpeedAction.Accelerate;
            else if (kb.IsKeyDown(Keys.S))
                speedAction = SpeedAction.Reverse;
            else
                speedAction = SpeedAction.Coast;

            if (kb.IsKeyDown(Keys.A))
                turningAction = TurningAction.TurnLeft;
            else if (kb.IsKeyDown(Keys.D))
                turningAction = TurningAction.TurnRight;
            else
                turningAction = TurningAction.None;

            return (speedAction, turningAction);
        }
    }
}
