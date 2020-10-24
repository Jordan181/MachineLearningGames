using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;

namespace MachineLearningGames.Framework.GeneticTraining
{
    public static class GeneticAlgorithmSteps
    {
        public static IReadOnlyList<(Weights weights, int score)> KeepFittest(IReadOnlyList<(int id, int score)> lastGenerationScores, Dictionary<int, Weights> currentGenerationWeights, int count, double scoreThreshold)
        {
            var orderedScores = lastGenerationScores.OrderByDescending(pair => pair.score);
            return orderedScores
                .Take(count)
                .Where(item => item.score > scoreThreshold)
                .Select(item =>
                {
                    var weights = currentGenerationWeights[item.id];
                    return (weights, item.score);
                })
                .ToList();
        }

        public static IReadOnlyList<Weights> GenerateChildren(IReadOnlyList<(Weights weights, int score)> fittest, double mutationRate, int count, NeuralNetworkSpecification specification)
        {
            var children = new List<Weights>(count);
            var fittestCumulative = WeightsChildGenerator.CalculateCumulativeScores(fittest);

            for (var i = 0; i < count; i++)
            {
                var (parent1, parent2) = WeightsChildGenerator.SelectParents(fittestCumulative); 

                var hiddenWeights = parent1.Hidden.Select((h, j) => WeightsChildGenerator.Merge(h, parent2.Hidden[j], mutationRate, specification)).ToList();
                var biasWeights = parent1.Bias.Select((h, j) => WeightsChildGenerator.Merge(h.ToColumnMatrix(), parent2.Bias[j].ToColumnMatrix(), mutationRate, specification).Column(0)).ToList();
                var outputWeights = WeightsChildGenerator.Merge(parent1.Output, parent2.Output, mutationRate, specification);

                children.Add(new Weights(hiddenWeights, biasWeights, outputWeights));
            }

            return children;
        }

        public static IReadOnlyList<Weights> GenerateRandom(RandomWeightsGenerator weightsGenerator, int count)
        {
            var weights = new List<Weights>(count);

            for (var i = 0; i < count; i++)
            {
                weights.Add(weightsGenerator.GenerateRandomWeights());
            }

            return weights;
        }

        public static Dictionary<int, Weights> CreateWeightsIdMap(IEnumerable<Weights> weights, int count)
        {
            var map = new Dictionary<int, Weights>(count);
            var id = 1;
            foreach (var weight in weights)
            {
                map.Add(id, weight);
                id++;
            }

            return map;
        }
    }
}
