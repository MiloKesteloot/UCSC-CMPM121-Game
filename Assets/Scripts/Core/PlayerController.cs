using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI[] spelluis;

    public int speed;

    public Unit unit;

    public ClassManager.ClassType classType;

    private Vector3 spawnPoint;

    public void UpdateStats() {
        this.hp.SetMaxHP((int) classType.GetHealth());
        this.spellcaster.max_mana = (int) classType.GetMana();
        this.spellcaster.mana_reg = (int) classType.GetManaRegeneration();
        this.speed = (int) classType.GetSpeed();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
        unit.OnMove += distance => StatsManager.Instance.AddStats(StatsManager.StatType.DistanceMoved, distance);
        spawnPoint = transform.position;

        classType = ClassManager.Instance.classTypes.Values.ToArray()[0];

        gameObject.transform.Find("SpriteGameObject").gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.playerSpriteManager.Get(classType.sprite);

        GameManager.Instance.playerController = this;
    }

    public void StartLevel()
    {
        transform.position = spawnPoint;

        spellcaster = new SpellCaster(125, 8, Hittable.Team.PLAYER, SpellBuilder.Build(this.spellcaster, "Magic Missile")); // SpellBuilder.BuildRandom(this.spellcaster)
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.OnDamage += damage => StatsManager.Instance.AddStats(StatsManager.StatType.DamageReceved, damage.amount);
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spelluis[0].SetSpell(spellcaster.spell);

        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StatsManager.Instance.SpellCast(spellcaster.spell.GetName());
        StatsManager.Instance.AddStats(StatsManager.StatType.SpellsCast, 1);
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*speed;
    }

    void Die()
    {
        Debug.Log("You Lost");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }
}
