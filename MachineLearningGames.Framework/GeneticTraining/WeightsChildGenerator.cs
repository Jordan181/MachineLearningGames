using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;
using MathNet.Numerics.LinearAlgebra;

namespace MachineLearningGames.Framework.GeneticTraining
{
    internal static class WeightsChildGenerator
    {
        public static IReadOnlyList<(Weights weights, int cumulativeScore)> CalculateCumulativeScores(IReadOnlyList<(Weights weights, int score)> fittest)
        {
            var score = 0;
            return fittest.Select(pair => (pair.weights, score += pair.score)).ToList();
        }

        public static (Weights parent1, Weights parent2) SelectParents(IReadOnlyList<(Weights weights, int cumulativeScore)> cumulativeFittest)
        {
            var rand = new Random();
            var totalScore = cumulativeFittest[^1].cumulativeScore;
            var score1 = rand.Next(totalScore);
            var score2 = rand.Next(score1, totalScore);

            Weights parent1 = null;
            Weights parent2 = null;
            foreach (var item in cumulativeFittest)
            {
                if (parent1 == null)
                {
                    if (item.cumulativeScore > score1)
                    {
                        parent1 = item.weights;
                    }

                    continue;
                }

                if (item.cumulativeScore >= score2)
                {
                    parent2 = item.weights;
                    break;
                }
            }
            
            return (parent1 ?? cumulativeFittest[0].weights, parent2 ?? cumulativeFittest[1].weights);
        }

        public static Matrix<float> Merge(Matrix<float> first, Matrix<float> second, double mutationRate, NeuralNetworkSpecification specification)
        {
            var rand = new Random();

            var flattenedFirst = first.ToRowMajorArray();
            var flattenedSecond = second.ToRowMajorArray();
            
            var length = flattenedFirst.Length;
            var child = new float[length];

            for (var i = 0; i < length; i++)
            {
                var mutation = (float)rand.NextDouble();
                if (mutation < mutationRate)
                    child[i] = RandomWeightsGenerator.GenerateRandomWeight(rand, specification.MaxWeight, specification.MinWeight);
                else
                {
                    var takeFromFirst = rand.NextDouble() < 0.5;
                    child[i] = takeFromFirst ? flattenedFirst[i] : flattenedSecond[i];
                }
            }
            
            return Matrix<float>.Build.DenseOfRowMajor(first.RowCount, first.ColumnCount, child);
        }
    }
}
