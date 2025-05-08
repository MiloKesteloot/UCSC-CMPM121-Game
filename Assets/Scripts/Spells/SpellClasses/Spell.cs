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

    public abstract int GetManaCost();

    public abstract int GetDamage();

    public abstract float GetCooldown();

    public abstract int GetIcon();

    public bool IsReady()
    {
        return last_cast + GetCooldown() < Time.time;
    }

    public abstract IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team);

    // TODO bullets aren't shooting in debug mode??

    public abstract class SpellInfo {
        public string name = "NO NAME";
        public string description = "NO DESCRIPTION PROVIDED";
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
