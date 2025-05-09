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
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team));
                Debug.Log("Delay: " + this.modifierSpellInfo.GetDelay());
                yield return new WaitForSeconds(this.modifierSpellInfo.GetDelay()); // TODO I feel like it's not waiting the right amount of time
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team));
                break;
            case "split":
                Debug.Log("Angle: " + this.modifierSpellInfo.GetAngle());
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, RotateVectorAroundVector(this.modifierSpellInfo.GetAngle(), target, where), team));
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, RotateVectorAroundVector(-this.modifierSpellInfo.GetAngle(), target, where), team));
                break;
            default:
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team));
                break;
        }
        yield return new WaitForEndOfFrame();
    }

    public Vector3 RotateVectorAroundVector(float a, Vector3 v, Vector3 c) {
        return Quaternion.AngleAxis(a, Vector3.forward) * (v - c) + c;
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

        public float GetDelay() {
            return RPN.Eval(delay, null);
        }

        public float GetAngle() {
            return RPN.Eval(angle, null);
        }
    }
}
