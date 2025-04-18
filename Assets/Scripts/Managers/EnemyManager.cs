using System.Collections.Generic;
using Newtonsoft.Json.Linq;
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

    public Dictionary<string, EnemyType> enemyTypes = new();

    private EnemyManager() {
        // TODO read through enemies.json and put EnemyInfo instances into enemyInfos
        var enemyText = Resources.Load<TextAsset>("enemies");

        JToken jo = JToken.Parse(enemyText.text);
        foreach (var enemy in jo) {
            EnemyType enemyType = enemy.ToObject<EnemyType>();
            enemyTypes[enemyType.name] = enemyType;
        }
    }

    public class EnemyType {
        public string name;
        public int sprite;
        public float hp;
        public float speed;
        public float damage;
    }
}
