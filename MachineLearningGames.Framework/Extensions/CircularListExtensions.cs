using System.Collections.Generic;

namespace MachineLearningGames.Framework.Extensions
{
    public static class CircularListExtensions
    {
        public static T NextOrFirst<T>(this IReadOnlyList<T> items, int current)
        {
            var index = (current + items.Count + 1) % items.Count;
            return items[index];
        }
        
        public static T PreviousOrLast<T>(this IReadOnlyList<T> items, int current)
        {
            var index = (current + items.Count - 1) % items.Count;
            return items[index];
        }
    }
}
