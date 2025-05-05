using System.Collections.Generic;

public static class SpellBuilder 
{

    public static Spell BuildBaseSpell(SpellCaster owner, string baseSpellName) {
        return new BaseSpell(owner, SpellManager.Instance.GetBaseSpellInfo(baseSpellName));
    }

    // public static Spell BuildModifierSpell(SpellCaster owner, string modifierSpellName, ) {
    //     return new ModifierSpell(owner, SpellManager.Instance.GetModifierSpellInfo(modifierSpellName));
    // }

    public static Spell Build(SpellCaster owner, BaseSpell.BaseSpellInfo baseSpellInfo, params ModifierSpell.ModifierSpellInfo[] modifierSpellInfos)
    {
        Spell spell = new BaseSpell(owner, baseSpellInfo);

        foreach (ModifierSpell.ModifierSpellInfo modifierSpellInfo in modifierSpellInfos) {
            ModifierSpell modifierSpell = new(owner, modifierSpellInfo, spell);
            spell = modifierSpell;
        }
        
        return spell;
    }

    public static Spell BuildRandom(SpellCaster owner) {
        List<ModifierSpell.ModifierSpellInfo> modifierSpellInfos = new();
        int saftey = 10000;
        while (saftey > 0) {
            Spell.SpellInfo spellInfo = SpellManager.Instance.GetSpellInfo("Random");
            if (spellInfo is BaseSpell.BaseSpellInfo) {
                return Build(owner, (BaseSpell.BaseSpellInfo) spellInfo, modifierSpellInfos.ToArray());
            } else {
                modifierSpellInfos.Add((ModifierSpell.ModifierSpellInfo) spellInfo);
            }
        }
        throw new KeyNotFoundException("Saftey Triggered! BaseSpell could not be found.");
    }
}
