using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ClassManager
{
    private static ClassManager theInstance = null;

    public static ClassManager Instance {  get
        {
            if (theInstance == null) {
                theInstance = new ClassManager();
            }
            return theInstance;
        }
    }

    public Dictionary<int, ClassType> classTypes = new();

    private ClassManager() {
        var classText = Resources.Load<TextAsset>("classes");

        JObject jo = JObject.Parse(classText.text);
        foreach (var c in jo) {
            ClassType classType = c.Value.ToObject<ClassType>();
            classTypes[classType.sprite] = classType;
        }
    }

    public class ClassType {
        public string name;
        public int sprite;
        public string health;
        public string mana;
        [JsonProperty("mana_regeneration")] public string manaRegeneration;
        public string spellpower;
        public string speed;

        public float GetHealth() {
            return RPN.Eval(health, null);
        }

        public float GetMana() {
            return RPN.Eval(mana, null);
        }

        public float GetManaRegeneration() {
            return RPN.Eval(manaRegeneration, null);
        }

        public float GetSpellpower() {
            return RPN.Eval(spellpower, null);
        }

        public float GetSpeed() {
            return RPN.Eval(speed, null);
        }
    }
}
