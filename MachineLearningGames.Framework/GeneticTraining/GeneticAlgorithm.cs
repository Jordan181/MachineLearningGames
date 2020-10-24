using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;

namespace MachineLearningGames.Framework.GeneticTraining
{
    public sealed class GeneticAlgorithm
    {
        private readonly int topToKeepCount;
        private readonly int childrenToGenerateCount;
        private readonly RandomWeightsGenerator randomWeightsGenerator;
        private readonly TrainingSpecification trainingSpecification;
        private readonly NeuralNetworkSpecification neuralNetworkSpecification;

        private Dictionary<int, Weights> currentGenerationWeights;
        
        public GeneticAlgorithm(
            RandomWeightsGenerator randomWeightsGenerator,
            TrainingSpecification trainingSpecification,
            NeuralNetworkSpecification neuralNetworkSpecification)
        {
            this.randomWeightsGenerator = randomWeightsGenerator;
            this.trainingSpecification = trainingSpecification;
            this.neuralNetworkSpecification = neuralNetworkSpecification;

            topToKeepCount = (int) (trainingSpecification.PopulationSize * trainingSpecification.TopFractionToKeep);
            childrenToGenerateCount = (int) (trainingSpecification.PopulationSize * trainingSpecification.ChildrenFractionToGenerate);
        }

        public IReadOnlyDictionary<int, Weights> EvolveWeights(IReadOnlyList<(int id, int score)> lastGenerationScores)
        {
            IEnumerable<Weights> weights;
            if (lastGenerationScores.Count == 0)
            {
                weights = GeneticAlgorithmSteps.GenerateRandom(randomWeightsGenerator, trainingSpecification.PopulationSize);
            }
            else
            {
                var fittest = GeneticAlgorithmSteps.KeepFittest(lastGenerationScores, currentGenerationWeights, topToKeepCount, trainingSpecification.FittestKeptScoreThreshold);
                var children = fittest.Count < 2 
                    ? new List<Weights>() 
                    : GeneticAlgorithmSteps.GenerateChildren(fittest, trainingSpecification.MutationRate, childrenToGenerateCount, neuralNetworkSpecification);

                var randomToGenerateCount = trainingSpecification.PopulationSize - fittest.Count - children.Count;
                var random = GeneticAlgorithmSteps.GenerateRandom(randomWeightsGenerator, randomToGenerateCount);

                weights = fittest.Select(f => f.weights).Concat(children).Concat(random);
            }

            currentGenerationWeights = GeneticAlgorithmSteps.CreateWeightsIdMap(weights, trainingSpecification.PopulationSize);
            return currentGenerationWeights;
        }
    }
}
