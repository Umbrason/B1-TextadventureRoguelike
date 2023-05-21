using System.Collections.Generic;
using System.Linq;

public static class EnumUtils
{
    public static T RandomElement<T>(this IReadOnlyCollection<T> collection)
    {
        var randIndex = UnityEngine.Random.Range(0, collection.Count);
        return collection.ElementAt(randIndex);
    }

    public static T[] RandomElements<T>(this IReadOnlyCollection<T> collection, int count)
    {
        var results = new T[count];
        if (collection.Count < count) return collection.ToArray();
        var hashset = new HashSet<int>();
        for (int i = 0; i < count; i++)
        {
            var randIndex = UnityEngine.Random.Range(0, collection.Count);
            while (hashset.Contains(randIndex)) randIndex = UnityEngine.Random.Range(0, collection.Count);
            hashset.Add(randIndex);
            results[i] = collection.ElementAt(randIndex);
        }
        return results;
    }
}