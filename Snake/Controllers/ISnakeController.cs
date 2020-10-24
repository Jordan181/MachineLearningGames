namespace Snake.Controllers
{
    internal interface ISnakeController
    {
        int Id { get; }

        Direction ChooseDirection(SnakeControllerInputs controllerInputs);
    }
}