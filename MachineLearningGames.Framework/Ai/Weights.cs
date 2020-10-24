using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace MachineLearningGames.Framework.Ai
{
    public sealed class Weights
    {
        public IReadOnlyList<Matrix<float>> Hidden { get; }
        public IReadOnlyList<Vector<float>> Bias { get; }
        public Matrix<float> Output { get; }

        public Weights(IReadOnlyList<Matrix<float>> hidden, IReadOnlyList<Vector<float>> bias, Matrix<float> output)
        {
            Hidden = hidden;
            Bias = bias;
            Output = output;
        }
    }
}
