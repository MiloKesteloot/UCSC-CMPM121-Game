using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform target;
    public int speed;
    public int damage;
    public Hittable hp;
    public HealthBar healthui;
    public bool dead;
    public bool ranged = false;
    public SpellCaster spellcaster;

    List<GameManager.GameState> deathStates = new(){GameManager.GameState.WAVEEND, GameManager.GameState.GAMEOVER, GameManager.GameState.PREGAME};

    public float last_attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameManager.Instance.player.transform;
        hp.OnDeath += Die;
        healthui.SetHealth(hp);
    }

    // Update is called once per frame
    void Update()
    {
        if (deathStates.Contains(GameManager.Instance.state)) {
            this.Die();
            return;
        }
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude < 2f)
        {
            DoAttack();
        }
        else
        {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }

        if (ranged) {
            StartCoroutine(spellcaster.Cast(transform.position, target.position));
        }
    }

    public void ToggleRange() {
        ranged = true;

        spellcaster = new SpellCaster(125, 3, Hittable.Team.MONSTERS);
        spellcaster.mana = 0;
        StartCoroutine(spellcaster.ManaRegeneration());
    }
    
    void DoAttack()
    {
        if (last_attack + 2 < Time.time)
        {
            last_attack = Time.time;
            target.gameObject.GetComponent<PlayerController>().hp.Damage(new Damage(damage, Damage.Type.PHYSICAL));
        }
    }

    void Die()
    {
        if (!dead)
        {
            if (!deathStates.Contains(GameManager.Instance.state)) {
                StatsManager.Instance.AddStats(StatsManager.StatType.EnemiesKilled, 1);
                Debug.Log("Thing Died!");
            }
            dead = true;
            GameManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
