using System;
using System.Collections.Generic;
using MachineLearningGames.Framework.Ai;
using MathNet.Numerics.LinearAlgebra;

namespace Snake.Controllers
{
    public static class SnakeNeuralNetworkSpecification
    {
        private const int NumberOfInputs = 7;
        private const int NumberOfOutputs = 4;
        private static readonly IReadOnlyList<int> HiddenLayerSizes = new List<int> { 8 };
        private const float MinWeight = -5.0f;
        private const float MaxWeight = 5.0f;
        private static readonly Func<Vector<float>, Vector<float>> HiddenLayersActivationFunction = ActivationFunctions.Sigmoid;
        private static readonly Func<Vector<float>, Vector<float>> OutputLayerActivationFunction = ActivationFunctions.Sigmoid;

        public static readonly NeuralNetworkSpecification Specification = new NeuralNetworkSpecification(
                NumberOfInputs,
                NumberOfOutputs,
                HiddenLayerSizes,
                MinWeight,
                MaxWeight,
                HiddenLayersActivationFunction,
                OutputLayerActivationFunction);
    }
}