using UnityEngine;
using System.Collections;

public abstract class Spell
{
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;
    public SpellInfo info;

    public Spell(SpellCaster owner, SpellInfo info)
    {
        this.owner = owner;
        this.info = info;
    }

    public string GetName()
    {
        return info.name;
    }

    public string GetDescription() {
        return info.description;
    }

    public abstract int GetManaCost(string mods = "");

    public abstract int GetDamage(string mods = "");

    public abstract float GetCooldown(string mods = "");

    public abstract float GetSpeed(string mods = "");

    public abstract string GetTrajectory();

    public abstract bool GetPiercing();

    public abstract int GetIcon();

    public bool IsReady()
    {
        return last_cast + GetCooldown() < Time.time;
    }

    public abstract BaseSpell GetBaseSpell();

    public abstract IEnumerator Cast(Transform where, Vector3 target, Hittable.Team team, int damage, float speed, string trajectory, bool piercing);

    // TODO bullets aren't shooting in debug mode??

    public abstract class SpellInfo {
        public string name = "NO NAME";
        public string description = "NO DESCRIPTION PROVIDED";

        public abstract void SetUp();
    }

    public class DamageInfo {
        public string amount;
        public string type;
    }

    public class ProjectileInfo {
        public string trajectory = "straight";
        public string speed;
        public int sprite = -1;
        public string lifetime = "9999999";
    }
}
