using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager
{
    public enum GameState
    {
        PREGAME,
        PREGAME2,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null)
                theInstance = new GameManager();

            return theInstance;
        }
    }

    public void GameOver()
    {
        GameManager.Instance.state = GameState.GAMEOVER;
        if (RewardsRelicManager.Instance != null)
        {
            RewardsRelicManager.Instance.mostRecentWave = -1;
        }
        RelicManager.Instance.Reset();
        foreach (SpellUI sui in this.playerController.spelluis)
        {
            sui.SetSpell(null);
        }
        EventBus.Instance.Reset();
        this.playerController.spellPower = 0;

        for (int i = 0; i < RelicUIManager.Instance.transform.childCount; i++)
        {
            GameObject.Destroy(RelicUIManager.Instance.transform.GetChild(i).gameObject);
        }
    }

    public GameObject player;
    public PlayerController playerController;

    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }

    public int wave;
    public string level;
    public LevelManager.LevelType GetLevel() => LevelManager.Instance.levelTypes[GameManager.Instance.level];

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    private GameManager()
    {
        enemies = new List<GameObject>();
        StatsManager sm = StatsManager.Instance;
        // Debug.Log(LevelManager.Instance.levelTypes["Easy"].spawnsDictionary["zombie"].hp);
    }
}
