using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

public class ModifierSpell : Spell
{
    public ModifierSpellInfo modifierSpellInfo;
    public Spell subSpell;

    public ModifierSpell(SpellCaster owner, ModifierSpellInfo modifierSpellInfo, Spell subSpell) : base(owner, modifierSpellInfo) {
        this.modifierSpellInfo = modifierSpellInfo;
        this.subSpell = subSpell;
    }

    public override int GetManaCost()
    {
        return subSpell.GetManaCost();
    }

    public override int GetDamage()
    {
        return subSpell.GetDamage();
    }

    public override float GetCooldown()
    {
        return subSpell.GetCooldown();
    }

    public override int GetIcon()
    {
        return subSpell.GetIcon();
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team) {
        switch (this.GetName()) {
            case "doubled":
                this.subSpell.Cast(where, target, team);
                yield return new WaitForSeconds(this.modifierSpellInfo.GetDelay());
                this.subSpell.Cast(where, target, team);
                break;
            case "split":
                break;
            default:
                this.subSpell.Cast(where, target, team);
                break;
        }
        yield return new WaitForEndOfFrame();
    }

    public class ModifierSpellInfo : SpellInfo {
        public string angle;
        public string delay;
        [JsonProperty("damage_multiplier")] public string damageMultiplier;
        [JsonProperty("damage_adder")] public string damageAdder;
        [JsonProperty("mana_multiplier")] public string manaMultiplier;
        [JsonProperty("mana_adder")] public string manaAdder;
        [JsonProperty("cooldown_multiplier")] public string cooldownMultiplier;
        [JsonProperty("cooldown_adder")] public string cooldownAdder;
        [JsonProperty("projectile_trajectory")] public string projectileTrajectory;

        public int GetDelay() {
            return (int) RPN.Eval(delay, null);
        }

        public int GetAngle() {
            return (int) RPN.Eval(angle, null);
        }
    }
}
