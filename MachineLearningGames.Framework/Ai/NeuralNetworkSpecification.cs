using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace MachineLearningGames.Framework.Ai
{
    public class NeuralNetworkSpecification
    {
        public int NumberOfInputs { get; }
        public int NumberOfOutputs { get; }
        public IReadOnlyList<int> HiddenLayerSizes { get; }
        public float MinWeight { get; }
        public float MaxWeight { get; }
        public Func<Vector<float>, Vector<float>> HiddenLayersActivationFunction { get; }
        public Func<Vector<float>, Vector<float>> OutputLayerActivationFunction { get; }

        public NeuralNetworkSpecification(
            int numberOfInputs, 
            int numberOfOutputs, 
            IReadOnlyList<int> hiddenLayerSizes, 
            float minWeight, 
            float maxWeight,
            Func<Vector<float>, Vector<float>> hiddenLayersActivationFunction, 
            Func<Vector<float>, Vector<float>> outputLayerActivationFunction)
        {
            NumberOfInputs = numberOfInputs;
            NumberOfOutputs = numberOfOutputs;
            HiddenLayerSizes = hiddenLayerSizes;
            MinWeight = minWeight;
            MaxWeight = maxWeight;
            HiddenLayersActivationFunction = hiddenLayersActivationFunction;
            OutputLayerActivationFunction = outputLayerActivationFunction;
        }
    }
}
