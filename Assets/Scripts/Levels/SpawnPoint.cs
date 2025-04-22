using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnName {
        RED, GREEN, BONE
    }

    private static readonly Dictionary<string, SpawnName> spawnNameDictionary = new(StringComparer.OrdinalIgnoreCase) { // StringComparer.OrdinalIgnoreCase was given by Perplexity AI
        { "red", SpawnName.RED },
        { "bone", SpawnName.BONE },
        { "green", SpawnName.GREEN },
    };

    public SpawnName kind;

    // TODO this function could be WAY more efficent by grouping spawns into lists in a dictionary at scene load
    public static SpawnPoint GetRandom(SpawnPoint[] spawnPoints, string selector) {
        string[] strings = selector.Split(" ");
        
        List<SpawnPoint> viableSpawns = new();
        if (strings.Length <= 1) {
            viableSpawns = spawnPoints.ToList();
        } else {
            List<SpawnName> spawnNames = new();
            foreach (string s in strings) {
                if (s == "random") continue;
                spawnNames.Add(spawnNameDictionary[s]);
            }
            foreach (SpawnPoint spawnPoint in spawnPoints) {
                if (spawnNames.Contains(spawnPoint.kind)) {
                    viableSpawns.Add(spawnPoint);
                }
            }
        }
        return viableSpawns[RND.Instance.Next(viableSpawns.Count)];
    }
}
