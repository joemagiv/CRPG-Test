using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Character statistics system for stat-based dialogue rolls
/// </summary>
public class CharacterStats : MonoBehaviour
{
    [System.Serializable]
    public class Stat
    {
        public string statName;
        public int baseValue = 10;
        public int currentValue;
        public int minValue = 1;
        public int maxValue = 20;
        
        public Stat(string name, int baseVal = 10)
        {
            statName = name;
            baseValue = baseVal;
            currentValue = baseVal;
        }
        
        /// <summary>
        /// Roll against this stat (similar to D&D style roll)
        /// </summary>
        public int Roll()
        {
            return Random.Range(1, 21) + currentValue;
        }
        
        /// <summary>
        /// Check if roll succeeds against a difficulty
        /// </summary>
        public bool CheckRoll(int difficulty)
        {
            int roll = Roll();
            return roll >= difficulty;
        }
    }
    
    [Header("Character Stats")]
    public List<Stat> stats = new List<Stat>();
    
    [Header("Stat Modifiers")]
    public Dictionary<string, int> temporaryModifiers = new Dictionary<string, int>();
    
    void Awake()
    {
        // Initialize stats if not already set up
        if (stats.Count == 0)
        {
            SetupDefaultStats();
        }
        else
        {
            // Ensure current values are set
            foreach (var stat in stats)
            {
                if (stat.currentValue == 0)
                {
                    stat.currentValue = stat.baseValue;
                }
            }
        }
    }
    
    void SetupDefaultStats()
    {
        // Add some common RPG stats
        stats.Add(new Stat("Strength", 12));
        stats.Add(new Stat("Dexterity", 10));
        stats.Add(new Stat("Constitution", 14));
        stats.Add(new Stat("Intelligence", 10));
        stats.Add(new Stat("Wisdom", 8));
        stats.Add(new Stat("Charisma", 12));
        stats.Add(new Stat("Perception", 11));
        stats.Add(new Stat("Luck", 7));
    }
    
    /// <summary>
    /// Get a stat by name
    /// </summary>
    public Stat GetStat(string statName)
    {
        foreach (var stat in stats)
        {
            if (stat.statName.Equals(statName, System.StringComparison.OrdinalIgnoreCase))
            {
                return stat;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Get the current value of a stat
    /// </summary>
    public int GetStatValue(string statName)
    {
        var stat = GetStat(statName);
        if (stat != null)
        {
            int value = stat.currentValue;
            // Apply temporary modifiers
            if (temporaryModifiers.ContainsKey(statName))
            {
                value += temporaryModifiers[statName];
            }
            return Mathf.Clamp(value, stat.minValue, stat.maxValue);
        }
        return 0;
    }
    
    /// <summary>
    /// Modify a stat permanently
    /// </summary>
    public void ModifyStat(string statName, int amount)
    {
        var stat = GetStat(statName);
        if (stat != null)
        {
            stat.currentValue += amount;
            stat.currentValue = Mathf.Clamp(stat.currentValue, stat.minValue, stat.maxValue);
        }
    }
    
    /// <summary>
    /// Add a temporary modifier to a stat
    /// </summary>
    public void AddTemporaryModifier(string statName, int amount)
    {
        if (temporaryModifiers.ContainsKey(statName))
        {
            temporaryModifiers[statName] += amount;
        }
        else
        {
            temporaryModifiers[statName] = amount;
        }
    }
    
    /// <summary>
    /// Remove a temporary modifier
    /// </summary>
    public void RemoveTemporaryModifier(string statName)
    {
        if (temporaryModifiers.ContainsKey(statName))
        {
            temporaryModifiers.Remove(statName);
        }
    }
    
    /// <summary>
    /// Perform a stat roll and return the result
    /// </summary>
    public int RollStat(string statName)
    {
        var stat = GetStat(statName);
        if (stat != null)
        {
            return stat.Roll();
        }
        return 0;
    }
    
    /// <summary>
    /// Check if a stat roll succeeds against a difficulty
    /// </summary>
    public bool CheckStatRoll(string statName, int difficulty)
    {
        var stat = GetStat(statName);
        if (stat != null)
        {
            return stat.CheckRoll(difficulty);
        }
        return false;
    }
}