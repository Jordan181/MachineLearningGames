using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using Microsoft.Xna.Framework;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Driving2D.Controllers
{
    internal sealed class AiControllerInputBuilder
    {
        private readonly Car car;
        private readonly TrackManager trackManager;
        private readonly List<float> inputs;

        public AiControllerInputBuilder(Car car, TrackManager trackManager)
        {
            this.car = car;
            this.trackManager = trackManager;
            inputs = new List<float>();
        }

        public AiControllerInputBuilder LookAhead() => LookInDirection(0);
        public AiControllerInputBuilder LookAheadRight() => LookInDirection(45);
        public AiControllerInputBuilder LookRight() => LookInDirection(90);
        public AiControllerInputBuilder LookRightBehind() => LookInDirection(135);
        public AiControllerInputBuilder LookBehind() => LookInDirection(180);
        public AiControllerInputBuilder LookBehindLeft() => LookInDirection(-135);
        public AiControllerInputBuilder LookLeft() => LookInDirection(-90);
        public AiControllerInputBuilder LookLeftAhead() => LookInDirection(-45);

        public AiControllerInputBuilder AddCurrentSpeed()
        {
            inputs.Add(car.CurrentSpeed);
            return this;
        }

        public Vector<float> BuildInputs() => Vector.Build.DenseOfEnumerable(inputs);

        private AiControllerInputBuilder LookInDirection(float angleFromAhead)
        {
            var directionToLook = Vector2.Transform(car.DirectionVector, Matrix.CreateRotationZ(MathHelper.ToRadians(angleFromAhead)));
            var currentPosition = car.Position + directionToLook;
            var distance = 1;

            while (trackManager.IsOnTrack(currentPosition))
            {
                currentPosition += directionToLook;
                distance++;
            }

            inputs.Add(distance);

            return this;
        }
    }
}