using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;

namespace Driving2D.Controllers
{
    public class Driving2DAiController : IDriving2DController
    {
        private readonly NeuralNetwork neuralNetwork;

        public int Id { get; }

        public Driving2DAiController(int id, NeuralNetwork neuralNetwork)
        {
            this.neuralNetwork = neuralNetwork;
            Id = id;
        }
        
        public (SpeedAction speedAction, TurningAction turningAction) GetDrivingActions(Car car, TrackManager trackManager)
        {
            var inputs = new AiControllerInputBuilder(car, trackManager)
                .LookAhead()
                .LookAheadRight()
                .LookRight()
                .LookRightBehind()
                .LookBehind()
                .LookBehindLeft()
                .LookLeft()
                .LookLeftAhead()
                .AddCurrentSpeed()
                .BuildInputs();

            var outputs = neuralNetwork.GetOutputs(inputs);

            var speedAction = (SpeedAction) IndexOfMax(outputs.Take(3).ToList());
            var turningAction = (TurningAction) IndexOfMax(outputs.Skip(3).ToList());

            return (speedAction, turningAction);
        }

        private int IndexOfMax(IList<float> values)
        {
            var max = float.MinValue;
            var index = -1;
            for (var i = 0; i < values.Count; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    index = i;
                }
            }

            return index;
        }
    }
}