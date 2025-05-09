using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System;

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
            case "Arcane Pulse":
                return new ArcanePulseSpell(owner, baseSpellInfo);
            default:
                return new BaseSpell(owner, baseSpellInfo);
        }
    }

    public override int GetManaCost(string mods = "") {
        return (int) RPN.Eval(baseSpellInfo.manaCost + mods, null);
    }

    public override int GetDamage(string mods = "") {
        int i = (int) RPN.Eval(baseSpellInfo.damage.amount + mods, null);
        return i;
    }

    public override float GetCooldown(string mods = "") {
        return (int) RPN.Eval(baseSpellInfo.cooldown + mods, null);
    }

    public override float GetSpeed(string mods = "") {
        return RPN.Eval(baseSpellInfo.projectile.speed + mods, null);
    }

    public override string GetTrajectory() {
        return baseSpellInfo.projectile.trajectory ?? "straight";
    }

    public override int GetIcon() {
        return baseSpellInfo.icon;
    }

    public override IEnumerator Cast(Transform where, Vector3 target, Hittable.Team team, int damage, float speed, string trajectory) {
        last_cast = Time.time;
        this.team = team;
        ProjectileInfo p = baseSpellInfo.projectile;
        GameManager.Instance.projectileManager.CreateProjectile(p.sprite, trajectory, where.position, target - where.position, speed, OnHit(damage), RPN.Eval(p.lifetime, null));
        yield return new WaitForEndOfFrame();
    }

    public override BaseSpell GetBaseSpell() {
        return this;
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

    public virtual Action<Hittable,Vector3> OnHit(int damage) {
        void OnHit(Hittable other, Vector3 impact) {
            if (other.team != team) {
                other.Damage(new Damage(damage, Damage.Type.ARCANE)); // TODO make damageType accurate
            }
        }
        return OnHit;
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

        public override void SetUp() {}
    }
}