using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Custom Lua functions for stat-based dialogue conditions
/// </summary>
public class StatBasedDialogueConditions : MonoBehaviour
{
    void OnEnable()
    {
        // Register custom Lua functions
        Lua.RegisterFunction("CheckStatRoll", this, SymbolExtensions.GetMethodInfo(() => CheckStatRoll(string.Empty, 0)));
        Lua.RegisterFunction("GetStatValue", this, SymbolExtensions.GetMethodInfo(() => GetStatValue(string.Empty)));
        Lua.RegisterFunction("RollStat", this, SymbolExtensions.GetMethodInfo(() => RollStat(string.Empty)));
    }
    
    void OnDisable()
    {
        // Unregister custom Lua functions
        Lua.UnregisterFunction("CheckStatRoll");
        Lua.UnregisterFunction("GetStatValue");
        Lua.UnregisterFunction("RollStat");
    }
    
    /// <summary>
    /// Check if a stat roll succeeds against a difficulty
    /// Usage in Dialogue System: CheckStatRoll("Strength", 15)
    /// </summary>
    public bool CheckStatRoll(string statName, int difficulty)
    {
        // Get player's CharacterStats component
        Transform playerTransform = DialogueManager.CurrentActor;
        GameObject player = playerTransform != null ? playerTransform.gameObject : null;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            var stats = player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                return stats.CheckStatRoll(statName, difficulty);
            }
        }
        
        Debug.LogWarning("No CharacterStats component found for stat roll check: " + statName);
        return false;
    }
    
    /// <summary>
    /// Get the current value of a stat
    /// Usage in Dialogue System: GetStatValue("Intelligence")
    /// </summary>
    public int GetStatValue(string statName)
    {
        // Get player's CharacterStats component
        Transform playerTransform = DialogueManager.CurrentActor;
        GameObject player = playerTransform != null ? playerTransform.gameObject : null;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            var stats = player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                return stats.GetStatValue(statName);
            }
        }
        
        Debug.LogWarning("No CharacterStats component found for stat value check: " + statName);
        return 0;
    }
    
    /// <summary>
    /// Roll a stat and return the result
    /// Usage in Dialogue System: RollStat("Charisma")
    /// </summary>
    public int RollStat(string statName)
    {
        // Get player's CharacterStats component
        Transform playerTransform = DialogueManager.CurrentActor;
        GameObject player = playerTransform != null ? playerTransform.gameObject : null;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            var stats = player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                return stats.RollStat(statName);
            }
        }
        
        Debug.LogWarning("No CharacterStats component found for stat roll: " + statName);
        return 0;
    }
}