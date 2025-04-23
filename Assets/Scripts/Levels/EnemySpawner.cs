using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;
    private int coroutines = 0;

    void Start() {
        int y = 130;
        foreach (LevelManager.LevelType levelType in LevelManager.Instance.levelTypes.Values) {
            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, y);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(levelType.name);
            y -= 50;
        }
    }

    public void StartLevel(string levelName) {
        StatsManager.Instance.NewGame();
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.wave = 0;
        GameManager.Instance.level = levelName;
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        StartCoroutine(SpawnWave());
    }

    public void NextWave() {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave() {
        StatsManager.Instance.NewWave();
        GameManager.Instance.wave += 1;
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        coroutines = EnemyManager.Instance.enemyTypes.Keys.Count;
        foreach (string enemyName in EnemyManager.Instance.enemyTypes.Keys) {
            StartCoroutine(SpawnEnemyWave(enemyName));
        }
        
        yield return new WaitWhile(() => coroutines > 0 || GameManager.Instance.enemy_count > 0);

        if (GameManager.Instance.state != GameManager.GameState.GAMEOVER) {
            if (GameManager.Instance.wave == GameManager.Instance.GetLevel().waves) {
                GameManager.Instance.state = GameManager.GameState.GAMEOVER;
            } else {
                GameManager.Instance.state = GameManager.GameState.WAVEEND;
            }
        }
    }

    IEnumerator SpawnEnemyWave(string enemyName) {
        LevelManager.Spawn spawn = GameManager.Instance.GetLevel().spawnsDictionary[enemyName];
        int count = RPN.EvalBase(spawn.count);
        int delay = RPN.EvalBase(spawn.delay);
        List<int> sequence = spawn.sequence;
        int sequenceIndex = 0;

        string location = spawn.location;

        while (count > 0) {
            SpawnPoint spawn_point = SpawnPoint.GetRandom(SpawnPoints, location);
            for (int i = 0; i < sequence[sequenceIndex]; i++) {
                Vector2 offset = Random.insideUnitCircle * 1.8f;
                Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);

                SpawnEnemy(enemyName, initial_position);

                count--;
                if (count <= 0) break;
            }
            if (count <= 0) break;
            sequenceIndex += 1;
            sequenceIndex %= sequence.Count;
            yield return new WaitForSeconds(delay);
        }
        coroutines -= 1;
    }

    public void SpawnEnemy(string enemyName, Vector3 location) {
        EnemyManager.EnemyType enemyType = EnemyManager.Instance.enemyTypes[enemyName];
        LevelManager.Spawn spawn = GameManager.Instance.GetLevel().spawnsDictionary[enemyName];

        int hp     = RPN.EvalBase(spawn.hp,     enemyType.hp);
        int speed  = RPN.EvalBase(spawn.speed,  enemyType.speed);
        int damage = RPN.EvalBase(spawn.damage, enemyType.damage);

        GameObject new_enemy = Instantiate(enemy, location, Quaternion.identity);
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemyType.sprite);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp     = new Hittable(hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed  = speed;
        en.damage = damage;

        GameManager.Instance.AddEnemy(new_enemy);
    }
}
