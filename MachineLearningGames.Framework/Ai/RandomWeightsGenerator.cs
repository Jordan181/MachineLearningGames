using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace MachineLearningGames.Framework.Ai
{
    public sealed class RandomWeightsGenerator
    {
        private readonly NeuralNetworkSpecification specification;

        public RandomWeightsGenerator(NeuralNetworkSpecification specification)
        {
            this.specification = specification;
        }

        public static float GenerateRandomWeight(Random rand, float maxWeight, float minWeight) => (float)(rand.NextDouble() * (maxWeight - minWeight) - maxWeight);
        
        public Weights GenerateRandomWeights()
        {
            var hiddenLayersCount = specification.HiddenLayerSizes.Count;

            var hiddenWeights = new List<Matrix<float>>(hiddenLayersCount);
            var biasWeights = new List<Vector<float>>(hiddenLayersCount + 1);

            for (var i = 0; i < hiddenLayersCount; i++)
            {
                int rows, columns;

                if (i == 0)
                {
                    rows = specification.HiddenLayerSizes[i];
                    columns = specification.NumberOfInputs;
                }
                else
                {
                    rows = specification.HiddenLayerSizes[i];
                    columns = specification.HiddenLayerSizes[i - 1];
                }

                hiddenWeights.Add(CreateRandomWeightsMatrix(rows, columns));
                biasWeights.Add(CreateRandomBiasWeights(rows));
            }

            biasWeights.Add(CreateRandomBiasWeights(specification.NumberOfOutputs));
            var outputWeights = CreateRandomWeightsMatrix(specification.NumberOfOutputs, specification.HiddenLayerSizes[^1]);

            return new Weights(hiddenWeights, biasWeights, outputWeights);
        }

        private Matrix<float> CreateRandomWeightsMatrix(int rows, int columns)
        {
            var rand = new Random();
            var weightsMatrix = Matrix.Build.Dense(rows, columns);

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    weightsMatrix[i, j] = GenerateRandomWeight(rand, specification.MaxWeight, specification.MinWeight);
                }
            }

            return weightsMatrix;
        }

        private Vector<float> CreateRandomBiasWeights(int rows) => CreateRandomWeightsMatrix(rows, 1).Column(0);
    }
}
