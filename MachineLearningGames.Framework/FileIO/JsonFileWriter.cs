using System.IO;
using System.Net.Http.Headers;
using MachineLearningGames.Framework.Ai;
using Newtonsoft.Json;

namespace MachineLearningGames.Framework.FileIO
{
    public static class JsonFileWriter
    {
        public static void SerializeObject<T>(T item, string path)
        {
            using var writer = new StreamWriter(path);
            var json = JsonConvert.SerializeObject(item);
            writer.Write(json);
        }

        public static void SerializeWeights(Weights weights, string path)
        {
            var serializableWeights = SerializableWeights.FromWeights(weights);
            SerializeObject(serializableWeights, path);
        }
    }
}
