using System;
using System.Collections.Generic;
using Driving2D.Controllers;
using Microsoft.Xna.Framework;

namespace Driving2D
{
    public class Car
    {
        private const float MaxSpeed = 8f;
        private const float Acceleration = 0.1f;
        private const float DragFactor = 0.001f;
        private const float BrakingForce = 0.1f;
        private const float TurnSpeed = 0.0002f;

        private readonly int width;
        private readonly int height;

        private float currentTurningAngle;
        private Vector2 perpendicularDirectionVector;

        public Vector2[] Corners => new []
        {
            Position,
            Position + perpendicularDirectionVector * width, 
            Position - DirectionVector * height, 
            Position - DirectionVector * height + perpendicularDirectionVector * width
        };

        public IDriving2DController Controller { get; }
        public List<TimeSpan> AllLapTimes { get; }
        public Vector2 Position { get; private set; }
        public float Direction { get; private set; }
        public Vector2 DirectionVector { get; private set; }
        public float CurrentSpeed { get; private set; }
        public bool IsBraking { get; private set; }
        public bool IsAlive { get; set; }
        public TimeSpan CurrentLapTime { get; set; }
        public int TargetCheckPoint { get; set; }

        public Car(Vector2 initialPosition, Vector2 initialDirection, IDriving2DController controller, int width, int height)
        {
            this.width = width;
            this.height = height;

            Controller = controller;
            AllLapTimes = new List<TimeSpan>();
            Position = initialPosition;
            Direction = MathF.Acos(Vector2.Dot(-Vector2.UnitY, Vector2.Normalize(initialDirection)));
            DirectionVector = initialDirection;
            CurrentSpeed = 0;
            IsAlive = true;
            CurrentLapTime = TimeSpan.Zero;
            TargetCheckPoint = 1;
            currentTurningAngle = 0;
        }

        public void Update(GameTime gameTime, SpeedAction speedAction, TurningAction turningAction)
        {
            CurrentLapTime += gameTime.ElapsedGameTime;

            UpdateDirection(turningAction);
            UpdatePosition(speedAction);
        }

        private void UpdateDirection(TurningAction turningAction)
        {
            switch (turningAction)
            {
                case TurningAction.TurnRight:
                {
                    if (currentTurningAngle < 0)
                        currentTurningAngle += TurnSpeed * 10;
                    else
                        currentTurningAngle += TurnSpeed;
                    break;
                }
                case TurningAction.TurnLeft:
                {
                    if (currentTurningAngle > 0)
                        currentTurningAngle -= TurnSpeed * 10;
                    else
                        currentTurningAngle -= TurnSpeed;
                    break;
                }
                case TurningAction.None:
                {
                    if (Math.Abs(Direction) > 0)
                        currentTurningAngle = MathHelper.Lerp(currentTurningAngle, 0, 0.1f);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(turningAction), turningAction, null);
            }

            Direction = MathHelper.Lerp(Direction, Direction + currentTurningAngle, CurrentSpeed);
            DirectionVector = Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(Direction));
            perpendicularDirectionVector = new Vector2(-DirectionVector.Y, DirectionVector.X);
        }

        private void UpdatePosition(SpeedAction speedAction)
        {
            IsBraking = false;

            switch (speedAction)
            {
                case SpeedAction.Accelerate:
                {
                    if (CurrentSpeed < 0)
                    {
                        IsBraking = true;
                        CurrentSpeed += BrakingForce;
                    }
                    else if (CurrentSpeed < MaxSpeed)
                        CurrentSpeed += Acceleration;
                    break;
                }
                case SpeedAction.Reverse:
                {
                    if (CurrentSpeed > 0)
                    {
                        IsBraking = true;
                        CurrentSpeed -= BrakingForce;
                    }
                    else if (CurrentSpeed > -MaxSpeed)
                        CurrentSpeed -= Acceleration;
                    break;
                }
                case SpeedAction.Coast:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(speedAction), speedAction, null);
            }

            CurrentSpeed += -DragFactor * CurrentSpeed * MathF.Abs(CurrentSpeed);
            Position += DirectionVector * CurrentSpeed;
        }
    }
}
