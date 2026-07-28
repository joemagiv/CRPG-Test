using UnityEngine;

/// <summary>
/// Ensures an EventSystem exists in the scene for UI and input handling
/// Works with both legacy and new Input System
/// </summary>
public class EventSystemSetup : MonoBehaviour
{
    void Awake()
    {
        InputSystemCompatibility.EnsureCompatibleEventSystem();
    }
}