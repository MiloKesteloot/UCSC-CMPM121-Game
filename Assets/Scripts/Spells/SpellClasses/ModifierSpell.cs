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

    public override int GetManaCost(string mods = "") {
        return subSpell.GetManaCost(modifierSpellInfo.manaMultiplier + modifierSpellInfo.damageAdder + mods);
    }

    public override int GetDamage(string mods = "") {
        return subSpell.GetDamage(modifierSpellInfo.damageMultiplier + modifierSpellInfo.damageAdder + mods);
    }

    public override float GetCooldown(string mods = "") {
        return subSpell.GetCooldown(modifierSpellInfo.cooldownMultiplier + modifierSpellInfo.cooldownAdder + mods);
    }

    public override float GetSpeed(string mods = "") {
        return subSpell.GetSpeed(modifierSpellInfo.speedMultiplier + modifierSpellInfo.speedAdder + mods);
    }

    public override string GetTrajectory() {
        return modifierSpellInfo.projectileTrajectory ?? subSpell.GetTrajectory();
    }

    public override int GetIcon() {
        return subSpell.GetIcon();
    }

    

    public override IEnumerator Cast(Transform where, Vector3 target, Hittable.Team team, int damage, float speed, string trajectory) {
        last_cast = Time.time;
        switch (this.GetName()) {
            case "doubled":
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team, damage, speed, trajectory));
                yield return new WaitForSeconds(this.modifierSpellInfo.GetDelay());
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team, damage, speed, trajectory));
                break;
            case "tripled":
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team, damage, speed, trajectory));
                yield return new WaitForSeconds(this.modifierSpellInfo.GetDelay());
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team, damage, speed, trajectory));
                yield return new WaitForSeconds(this.modifierSpellInfo.GetDelay());
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team, damage, speed, trajectory));
                break;
            case "split":
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, RotateVectorAroundVector(this.modifierSpellInfo.GetAngle() + Random.value*6-3, target, where.position), team, damage, speed, trajectory));
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, RotateVectorAroundVector(-this.modifierSpellInfo.GetAngle() + Random.value*6-3, target, where.position), team, damage, speed, trajectory));
                break;
            default:
                CoroutineManager.Instance.StartCoroutine(this.subSpell.Cast(where, target, team, damage, speed, trajectory));
                break;
        }
        yield return new WaitForEndOfFrame();
    }

    public override BaseSpell GetBaseSpell() {
        return this.subSpell.GetBaseSpell();
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
        [JsonProperty("speed_multiplier")] public string speedMultiplier;
        [JsonProperty("speed_adder")] public string speedAdder;
        [JsonProperty("projectile_trajectory")] public string projectileTrajectory;

        public override void SetUp() {
            if (damageMultiplier == null) damageMultiplier = "";
            else damageMultiplier = $" {damageMultiplier} *";

            if (damageAdder == null) damageAdder = "";
            else damageAdder = $" {damageAdder} +";

            if (manaMultiplier == null) manaMultiplier = "";
            else manaMultiplier = $" {manaMultiplier} *";

            if (manaAdder == null) manaAdder = "";
            else manaAdder = $" {manaAdder} +";

            if (cooldownMultiplier == null) cooldownMultiplier = "";
            else cooldownMultiplier = $" {cooldownMultiplier} *";

            if (cooldownAdder == null) cooldownAdder = "";
            else cooldownAdder = $" {cooldownAdder} +";

            if (speedMultiplier == null) speedMultiplier = "";
            else speedMultiplier = $" {speedMultiplier} *";

            if (speedAdder == null) speedAdder = "";
            else speedAdder = $" {speedAdder} +";
        }

        public float GetDelay() {
            return RPN.Eval(delay, null);
        }

        public float GetAngle() {
            return RPN.Eval(angle, null);
        }
    }
}
