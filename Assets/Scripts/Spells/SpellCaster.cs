using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public Hittable.Team team;
    public Spell spell;

    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }

    public SpellCaster(int mana, int mana_reg, Hittable.Team team, Spell spell)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;
        this.spell = spell;
    }

    public void AddMana(int amount)
    {
        this.mana += amount;
        if (this.mana > this.max_mana) this.mana = this.max_mana;
        if (this.mana < 0) this.mana = 0;
    }

    public IEnumerator Cast(Transform where, Vector3 target)
    {
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spell.GetManaCost();
            yield return spell.Cast(where, target, team, spell.GetDamage(), spell.GetSpeed(), spell.GetTrajectory(), spell.GetPiercing());
        }
        yield break;
    }

}
