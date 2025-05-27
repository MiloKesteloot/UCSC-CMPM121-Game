using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RelicManager
{
    private static RelicManager theInstance = null;

    public static RelicManager Instance
    {
        get
        {
            if (theInstance == null)
            {
                theInstance = new RelicManager();
            }
            return theInstance;
        }
    }

    public Dictionary<string, RelicType> relicTypes = new();

    private RelicManager()
    {
        var relicText = Resources.Load<TextAsset>("relics");

        JToken jo = JToken.Parse(relicText.text);
        foreach (var relic in jo)
        {
            RelicType relicType = relic.ToObject<RelicType>();
            relicTypes[relicType.name] = relicType;
        }
    }

    public class RelicType
    {
        public string name;
        public int sprite;
        public TriggerInfo trigger;
        public EffectInfo effect;
    }

    public class TriggerInfo
    {
        public string description;
        public string type;
        public string amount;
    }

    public class EffectInfo
    {
        public string description;
        public string type;
        public string amount;
        public string until;
    }
}
