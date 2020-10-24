using System.Collections.Generic;
using MachineLearningGames.Framework.Ai;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace TestAiTrainer
{
    internal static class TestHelpers
    {
        public static Weights CreateTestWeights() => new Weights(new List<Matrix<float>>(), new List<Vector<float>>(), new DenseMatrix(0));
    }
}
