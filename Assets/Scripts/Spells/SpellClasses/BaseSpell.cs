using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class BaseSpell : Spell
{   
    public BaseSpellInfo baseSpellInfo; // Same object as info

    public BaseSpell(SpellCaster owner, BaseSpellInfo baseSpellInfo) : base(owner, baseSpellInfo) {
        this.baseSpellInfo = baseSpellInfo;
    }

    public override int GetManaCost() {
        return (int) RPNEvaluator.Eval(baseSpellInfo.manaCost, null);
    }

    public override int GetDamage() {
        return (int) RPNEvaluator.Eval(baseSpellInfo.damage.amount, null);
    }

    public override float GetCooldown() {
        return (int) RPNEvaluator.Eval(baseSpellInfo.cooldown, null);
    }

    public override int GetIcon() {
        return baseSpellInfo.icon;
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team) {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
        yield return new WaitForEndOfFrame();
    }

    public class BaseSpellInfo : SpellInfo {
        public int icon = -1;
        public string N;
        public string spray;
        public DamageInfo damage;
        [JsonProperty("secondary_damage")] public string secondaryDamage;
        [JsonProperty("mana_cost")] public string manaCost;
        public string cooldown;
        public ProjectileInfo projectile;
        [JsonProperty("secondary_projectile")] public ProjectileInfo secondaryProjectile;
    }
}