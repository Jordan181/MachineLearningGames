using System;
using FlappyBird.Control;
using MachineLearningGames.Framework.Ai;
using MathNet.Numerics.LinearAlgebra.Single;

namespace FlappyBird.Controllers
{
    internal sealed class FlappyBirdAiController : IFlappyBirdController
    {
        private readonly NeuralNetwork neuralNetwork;

        public int Id { get; }

        public FlappyBirdAiController(int id, NeuralNetwork neuralNetwork)
        {
            Id = id;
            this.neuralNetwork = neuralNetwork;
        }
        
        public bool Flap(FlappyBirdControllerInputs controllerInputs)
        {
            var aiInputs = Vector.Build.Dense(new[]
            {
                Convert.ToSingle(controllerInputs.CanFlap),
                controllerInputs.BirdY,
                controllerInputs.BirdSpeed,
                controllerInputs.PipeDistanceX,
                controllerInputs.PipeGapDistanceY
            });

            var outputs = neuralNetwork.GetOutputs(aiInputs);
            return outputs[1] > outputs[0];
        }
    }
}
