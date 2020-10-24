using System.IO;
using MachineLearningGames.Framework.Ai;
using Newtonsoft.Json;

namespace MachineLearningGames.Framework.FileIO
{
    public static class JsonFileReader
    {
        public static T ReadJson<T>(string filePath)
        {
            using var reader = new StreamReader(filePath);
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Weights LoadWeights(string filePath)
        {
            var serializableWeights = ReadJson<SerializableWeights>(filePath);
            return serializableWeights.ToWeights();
        }
    }
}
