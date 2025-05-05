using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using SpellInfo = Spell.SpellInfo;

public class SpellManager
{
    private static SpellManager theInstance = null;

    public static SpellManager Instance {  get
        {
            if (theInstance == null) {
                theInstance = new SpellManager();
            }
            return theInstance;
        }
    }

    public Dictionary<string, SpellInfo> spellInfos = new();
    public Dictionary<string, BaseSpell.BaseSpellInfo> baseSpellInfos = new();
    public Dictionary<string, ModifierSpell.ModifierSpellInfo> modifierSpellInfos = new();

    private SpellManager() {

        TextAsset spellText = Resources.Load<TextAsset>("spells");
        JObject jt = JObject.Parse(spellText.text);

        foreach (var spell in jt) {
            if (spell.Value["icon"] != null) {
                BaseSpell.BaseSpellInfo baseSpellInfo = spell.Value.ToObject<BaseSpell.BaseSpellInfo>();
                baseSpellInfos.Add(baseSpellInfo.name, baseSpellInfo);
                spellInfos.Add(baseSpellInfo.name, baseSpellInfo);
            } else {
                ModifierSpell.ModifierSpellInfo modifierSpellInfo = spell.Value.ToObject<ModifierSpell.ModifierSpellInfo>();
                modifierSpellInfos.Add(modifierSpellInfo.name, modifierSpellInfo);
                spellInfos.Add(modifierSpellInfo.name, modifierSpellInfo);
            }
        }
    }

    public SpellInfo GetSpellInfo(string name) {
        if (name == "Random") return DictionaryTools.GetRandomValue(spellInfos);
        return spellInfos[name];
    }

    public BaseSpell.BaseSpellInfo GetBaseSpellInfo(string name) {
        if (name == "Random") return DictionaryTools.GetRandomValue(baseSpellInfos);
        return baseSpellInfos[name];
    }

    public ModifierSpell.ModifierSpellInfo GetModifierSpellInfo(string name) {
        if (name == "Random") return DictionaryTools.GetRandomValue(modifierSpellInfos);
        return modifierSpellInfos[name];
    }
}
