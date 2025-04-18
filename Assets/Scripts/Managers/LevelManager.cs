using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class LevelManager
{
    private static LevelManager theInstance = null;

    public static LevelManager Instance {  get
        {
            if (theInstance == null) {
                theInstance = new LevelManager();
            }
            return theInstance;
        }
    }

    public Dictionary<string, LevelType> levelTypes = new();

    private LevelManager() {
        // TODO read through levels.json and put LevelInfo instances into levelInfos
        var levelText = Resources.Load<TextAsset>("levels");

        JToken jo = JToken.Parse(levelText.text);
        foreach (var level in jo) {
            LevelType levelType = level.ToObject<LevelType>();
            levelTypes[levelType.name] = levelType;

            levelType.spawnsDictionary = new();
            foreach (Spawn spawn in levelType.spawns) {
                levelType.spawnsDictionary[spawn.enemy] = spawn;
            }
        }
    }

    public class LevelType {
        public string name;
        public int waves;
        public List<Spawn> spawns;
        public Dictionary<string, Spawn> spawnsDictionary;
    }

    public class Spawn {
        public string enemy;
        public string count;
        public string hp;
        public string delay;
        public List<int> sequence;
        public string location;  
    }
}
