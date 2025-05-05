using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DictionaryTools {
    public static TValue GetRandomValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary) {
        int i = (int) (UnityEngine.Random.value * dictionary.Count);
        return dictionary.Values.ToList()[i];
    }
}