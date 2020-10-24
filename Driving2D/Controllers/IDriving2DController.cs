namespace Driving2D.Controllers
{
    public interface IDriving2DController
    {
        int Id { get; }

        (SpeedAction speedAction, TurningAction turningAction) GetDrivingActions(Car car, TrackManager trackManager);
    }
}