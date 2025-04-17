using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private static EnemyManager theInstance = null;

    public static EnemyManager Instance {  get
        {
            if (theInstance == null) {
                theInstance = new EnemyManager();
            }
            return theInstance;
        }
    }

    public List<EnemyInfo> enemyInfos = new List<EnemyInfo>();

    private EnemyManager() {
        // TODO read through enemies.json and put EnemyInfo instances into enemyInfos
    }

    public class EnemyInfo {
        public string name;
    }
}
