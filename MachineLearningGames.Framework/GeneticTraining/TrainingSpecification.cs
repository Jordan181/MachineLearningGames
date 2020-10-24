using System.Runtime.CompilerServices;

namespace MachineLearningGames.Framework.GeneticTraining
{
    public struct TrainingSpecification
    {
        public int PopulationSize { get; set; }
        public double TopFractionToKeep { get; set; }
        public double ChildrenFractionToGenerate { get; set; }
        public double MutationRate { get; set; }
        public double FittestKeptScoreThreshold { get; set; }
    }
}
