using UnityEngine;

/// <summary>
/// Sets up the character animator controller at runtime
/// </summary>
[RequireComponent(typeof(Animator))]
public class CharacterAnimatorSetup : MonoBehaviour
{
    public RuntimeAnimatorController animatorController;
    
    void Awake()
    {
        Animator animator = GetComponent<Animator>();
        
        if (animatorController != null)
        {
            animator.runtimeAnimatorController = animatorController;
        }
        else
        {
            // Try to load the default animator controller
            RuntimeAnimatorController defaultController = Resources.Load<RuntimeAnimatorController>("MasculineCharacterController");
            if (defaultController != null)
            {
                animator.runtimeAnimatorController = defaultController;
            }
            else
            {
                Debug.LogWarning("No animator controller assigned and could not load default controller.");
            }
        }
    }
}