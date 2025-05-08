using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class BaseSpell : Spell
{
    public BaseSpellInfo baseSpellInfo; // Same object as info

    protected BaseSpell(SpellCaster owner, BaseSpellInfo baseSpellInfo) : base(owner, baseSpellInfo) {
        this.baseSpellInfo = baseSpellInfo;
    }

    public static BaseSpell Build(SpellCaster owner, BaseSpellInfo baseSpellInfo) {
        switch (baseSpellInfo.name) {
            case "Arcane Blast":
                return new ArcaneBlastSpell(owner, baseSpellInfo);
            case "Arcane Spray":
                return new ArcaneSpraySpell(owner, baseSpellInfo);
            default:
                return new BaseSpell(owner, baseSpellInfo);
        }
    }

    public override int GetManaCost() {
        return (int) RPN.Eval(baseSpellInfo.manaCost, null);
    }

    public override int GetDamage() {
        return (int) RPN.Eval(baseSpellInfo.damage.amount, null);
    }

    public override float GetCooldown() {
        return (int) RPN.Eval(baseSpellInfo.cooldown, null);
    }

    public override int GetIcon() {
        return baseSpellInfo.icon;
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team) {
        this.team = team;
        ProjectileInfo p = baseSpellInfo.projectile;
        GameManager.Instance.projectileManager.CreateProjectile(p.sprite, p.trajectory, where, target - where, RPN.Eval(p.speed, null), OnHit, RPN.Eval(p.lifetime, null));
        yield return new WaitForEndOfFrame();
    }

    //     if (this.baseSpellInfo.secondaryProjectile != null) {
    //         GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
    //     } else if (baseSpellInfo.N != null && RPN.Eval(this.baseSpellInfo.N, null) > 0) {
    //         int N = (int) RPN.Eval(this.baseSpellInfo.N, null);
    //         for (int i = 0; i < N; i++) {
    //             GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
    //         }
    //     } else {
            
    //     }

    public virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team) {
            other.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
        }
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