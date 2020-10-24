using System.Collections.Generic;
using MachineLearningGames.Framework.Ai;
using MachineLearningGames.Framework.GeneticTraining;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFramework.GeneticTraining
{

    [TestClass]
    public class GeneticAlgorithmStepsTest
    {
        [TestMethod]
        public void KeepFittest_should_only_take_correct_number_of_weights()
        {
            // Arrange
            var scores = CreateTestScores();
            var weights = CreateWeightsDictionary();

            const int count = 3;
            const double scoreThreshold = 0.0;

            // Act
            var fittest = GeneticAlgorithmSteps.KeepFittest(scores, weights, count, scoreThreshold);

            // Assert
            Assert.AreEqual(count, fittest.Count);
        }

        [TestMethod]
        public void KeepFittest_should_only_take_weights_with_score_greater_than_threshold()
        {
            // Arrange
            var scores = CreateTestScores();
            var weights = CreateWeightsDictionary();

            const int count = 3;
            const double scoreThreshold = 3.5;

            // Act
            var fittest = GeneticAlgorithmSteps.KeepFittest(scores, weights, count, scoreThreshold);

            // Assert
            Assert.AreEqual(2, fittest.Count);
        }

        [TestMethod]
        public void KeepFittest_should_return_empty_list_if_no_scores_greater_than_threshold()
        {
            // Arrange
            var scores = CreateTestScores();
            var weights = CreateWeightsDictionary();

            const int count = 3;
            const double scoreThreshold = 6;

            // Act
            var fittest = GeneticAlgorithmSteps.KeepFittest(scores, weights, count, scoreThreshold);

            // Assert
            Assert.AreEqual(0, fittest.Count);
        }

        private static Weights CreateTestWeights() => new Weights(new List<Matrix<float>>(), new List<Vector<float>>(), new DenseMatrix(0));

        private static List<(int id, int score)> CreateTestScores()
            => new List<(int id, int score)>
            {
                (1, 1),
                (2, 2),
                (3, 3),
                (4, 4),
                (5, 5)
            };

        private static Dictionary<int, Weights> CreateWeightsDictionary()
            => new Dictionary<int, Weights>
            {
                { 1, CreateTestWeights()},
                { 2, CreateTestWeights()},
                { 3, CreateTestWeights()},
                { 4, CreateTestWeights()},
                { 5, CreateTestWeights()}
            };
    }
}
