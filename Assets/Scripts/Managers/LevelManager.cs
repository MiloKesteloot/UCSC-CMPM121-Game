using System.Collections.Generic;
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

    public List<LevelInfo> levelInfos = new List<LevelInfo>();

    private LevelManager() {
        // TODO read through levels.json and put LevelInfo instances into levelInfos
    }

    public class LevelInfo {
        public string name;
    }
}
