using System;
using MathNet.Numerics.LinearAlgebra;

namespace MachineLearningGames.Framework.Ai
{
    public static class ActivationFunctions
    {
        public static Vector<float> Sigmoid(Vector<float> vector)
        {
            var newVector = Vector<float>.Build.Dense(vector.Count);

            for (var i = 0; i < vector.Count; i++)
                newVector[i] = 1 / (1 + (float)Math.Exp(-vector[i]));

            return newVector;
        }

        public static Vector<float> Tanh(Vector<float> vector) => vector.PointwiseTanh();

        public static float ExpMinusModX(float value) => MathF.Exp(-MathF.Abs(value));
    }
}
