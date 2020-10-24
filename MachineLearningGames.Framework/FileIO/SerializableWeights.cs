using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;
using MathNet.Numerics.LinearAlgebra.Single;

namespace MachineLearningGames.Framework.FileIO
{
    public sealed class SerializableWeights
    {
        public IReadOnlyList<float[,]> Hidden { get; }
        public IReadOnlyList<float[]> Bias { get; }
        public float[,] Output { get; }

        public SerializableWeights(IReadOnlyList<float[,]> hidden, IReadOnlyList<float[]> bias, float[,] output)
        {
            Hidden = hidden;
            Bias = bias;
            Output = output;
        }

        public static SerializableWeights FromWeights(Weights weights)
        {
            var hidden = weights.Hidden.Select(matrix => matrix.ToArray()).ToList();
            var bias = weights.Bias.Select(vector => vector.ToArray()).ToList();
            var output = weights.Output.ToArray();
            
            return new SerializableWeights(hidden, bias, output);
        }

        public Weights ToWeights()
        {
            var hidden = Hidden.Select(matrix => Matrix.Build.DenseOfArray(matrix)).ToList();
            var bias = Bias.Select(vector => Vector.Build.DenseOfArray(vector)).ToList();
            var output = Matrix.Build.DenseOfArray(Output);

            return new Weights(hidden, bias, output);
        }
    }
}