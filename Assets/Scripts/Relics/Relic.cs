using UnityEngine;
using System.Collections;
using RelicType = RelicManager.RelicType;

public abstract class Relic
{
    public PlayerController player;
    public RelicType relicType;

    public Relic(PlayerController player, RelicType relicType)
    {
        this.player = player;
        this.relicType = relicType;
    }

    public string GetName()
    {
        return info.name;
    }

    public string GetDescription() {
        return info.description;
    }

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
