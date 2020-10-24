using System;
using MachineLearningGames.Framework.FileIO;
using MathNet.Numerics.LinearAlgebra;

namespace MachineLearningGames.Framework.Ai
{
    public class NeuralNetwork
    {
        private readonly Weights weights;
        private readonly Func<Vector<float>, Vector<float>> hiddenLayerActivationFunction;
        private readonly Func<Vector<float>, Vector<float>> outputLayerActivationFunction;

        public NeuralNetwork(
            Weights weights, 
            Func<Vector<float>, Vector<float>> hiddenLayerActivationFunction,
            Func<Vector<float>, Vector<float>> outputLayerActivationFunction)
        {
            this.weights = weights;
            this.hiddenLayerActivationFunction = hiddenLayerActivationFunction;
            this.outputLayerActivationFunction = outputLayerActivationFunction;
        }

        public static NeuralNetwork CreateFromFile(NeuralNetworkSpecification specification, string filePath)
        {
            var weights = JsonFileReader.LoadWeights(filePath);
            return new NeuralNetwork(weights, specification.HiddenLayersActivationFunction, specification.OutputLayerActivationFunction);
        }

        public Vector<float> GetOutputs(Vector<float> inputs)
        {
            var z = inputs;

            for (var i = 0; i < weights.Hidden.Count; i++)
            {
                var hiddenNodes = weights.Hidden[i].Multiply(z);
                hiddenNodes = hiddenNodes.Add(weights.Bias[i]);
                z = hiddenLayerActivationFunction(hiddenNodes);
            }

            return outputLayerActivationFunction(weights.Output.Multiply(z));
        }
    }
}
