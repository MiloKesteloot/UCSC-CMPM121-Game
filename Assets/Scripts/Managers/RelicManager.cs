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

        this.Reset();
    }

    public List<RelicType> avalibleRelics = new();
    public void Reset()
    {
        avalibleRelics = new();
        foreach (var relic in relicTypes.Values)
        {
            avalibleRelics.Add(relic);
        }
    }
    public List<RelicType> GetXRandomRelics(int x)
    {
        if (avalibleRelics.Count < x)
        {
            this.Reset();
        }
        List<RelicType> tempList = new();
        while (tempList.Count < x)
        {
            RelicType randomRelic = avalibleRelics[UnityEngine.Random.Range(0, avalibleRelics.Count)];
            if (!tempList.Contains(randomRelic)) tempList.Add(randomRelic);
        }
        return tempList;
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
