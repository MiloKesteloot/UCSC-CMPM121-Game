using System.Collections.Generic;

public class StatsManager
{
    // TODO add a time spent in thing thing

    private static StatsManager theInstance = null;

    public static StatsManager Instance {  get
        {
            if (theInstance == null) {
                theInstance = new StatsManager();
            }
            return theInstance;
        }
    }

    public enum StatType {
        SpellsCast,
        EnemiesKilled,
        DamageReceved,
        DistanceMoved
    }

    public Dictionary<StatType, float> waveStats = new();
    public Dictionary<StatType, float> gameStats = new();
    public Dictionary<StatType, float> lifeStats = new();

    public Dictionary<string, int> weaponList = new();

    public void SpellCast(string name) {
        if (!weaponList.ContainsKey(name)) weaponList[name] = 0;
        weaponList[name]++;
    }

    public void AddStats(StatType statType, float value) {
        AddStats(waveStats, statType, value);
        AddStats(gameStats, statType, value);
        AddStats(lifeStats, statType, value);
    }
    public void AddStats(Dictionary<StatType, float> stats, StatType statType, float value) {
        if (!stats.ContainsKey(statType)) stats[statType] = 0;
        stats[statType] += value;
    }
    public int GetWaveStats(StatType statType) {
        return GetStats(waveStats, statType);
    }
    public int GetGameStats(StatType statType) {
        return GetStats(gameStats, statType);
    }
    public int GetLifeStats(StatType statType) {
        return GetStats(lifeStats, statType);
    }
    public int GetStats(Dictionary<StatType, float> stats, StatType statType) {
        if (!stats.ContainsKey(statType)) stats[statType] = 0;
        return (int) stats[statType];
    }

    public string GetFormattedWaveStats() {
        return  $"You beat wave {GameManager.Instance.wave}\n" +
                $"Favorite Spell: {GetFavoriteSpell()}\n" +
                $"Damage Receved: {GetWaveStats(StatType.DamageReceved)}\n" +
                $"Spells Cast: {GetWaveStats(StatType.SpellsCast)}\n" +
                $"Enemies Killed: {GetWaveStats(StatType.EnemiesKilled)}\n" +
                $"Distance Moved: {GetWaveStats(StatType.DistanceMoved)}";
    }

    public string GetFormattedGameStats() {
        return  $"Favorite Spell: {GetFavoriteSpell()}\n" +
                $"Damage Receved: {GetGameStats(StatType.DamageReceved)}\n" +
                $"Spells Cast: {GetGameStats(StatType.SpellsCast)}\n" +
                $"Enemies Killed: {GetGameStats(StatType.EnemiesKilled)}\n" +
                $"Distance Moved: {GetGameStats(StatType.DistanceMoved)}";
    }

    public string GetFavoriteSpell() {
        string favoriteSpell = "None";
        int maxValue = -1;
        foreach (var kvp in weaponList)
        {
            if (kvp.Value > maxValue)
            {
                maxValue = kvp.Value;
                favoriteSpell = kvp.Key;
            }
        }
        return favoriteSpell;
    }

    public void NewWave() {
        waveStats = new();
    }
    public void NewGame() {
        gameStats = new();
        weaponList = new();
    }
    public void NewLife() {
        lifeStats = new();
    }

}
