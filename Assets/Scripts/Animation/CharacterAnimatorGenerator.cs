using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Editor script to generate a comprehensive character animator controller
/// with idle, walk, and run animations for masculine characters
/// </summary>
public class CharacterAnimatorGenerator : EditorWindow
{
    private static readonly string ANIMATOR_PATH = "Assets/Animators/MasculineCharacterController.controller";
    private static readonly string ANIMATION_FOLDER = "Assets/Synty/AnimationBaseLocomotion/Samples/Animations/Polygon/Masculine/";
    
    [MenuItem("Tools/Generate Character Animator Controller")]
    public static void ShowWindow()
    {
        GetWindow<CharacterAnimatorGenerator>("Character Animator Generator");
    }
    
    [MenuItem("Tools/Generate Character Animator Controller/Generate Now")]
    public static void GenerateAnimatorController()
    {
        // Create animators folder if it doesn't exist
        string folderPath = "Assets/Animators";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Animators");
        }
        
        // Create a new animator controller
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(ANIMATOR_PATH);
        
        // Load existing animator controllers to reference their states
        AnimatorController idleController = AssetDatabase.LoadAssetAtPath<AnimatorController>(ANIMATION_FOLDER + "Idles/AC_Idle.controller");
        AnimatorController walkController = AssetDatabase.LoadAssetAtPath<AnimatorController>(ANIMATION_FOLDER + "Locomotion/Walks/AC_Walk_F.controller");
        AnimatorController runController = AssetDatabase.LoadAssetAtPath<AnimatorController>(ANIMATION_FOLDER + "Locomotion/Runs/AC_Run_F.controller");
        
        if (idleController == null || walkController == null || runController == null)
        {
            Debug.LogError("Could not load source animator controllers. Make sure the AnimationBaseLocomotion package is properly imported.");
            return;
        }
        
        // Get the motion from each controller
        Motion idleMotion = GetMotionFromController(idleController, "Idle");
        Motion walkMotion = GetMotionFromController(walkController, "Walk_F");
        Motion runMotion = GetMotionFromController(runController, "Run_F");
        
        if (idleMotion == null || walkMotion == null || runMotion == null)
        {
            Debug.LogError("Could not extract motions from source controllers.");
            return;
        }
        
        // Create parameters
        animatorController.AddParameter("Speed", AnimatorControllerParameterType.Float);
        animatorController.AddParameter("IsMoving", AnimatorControllerParameterType.Bool);
        
        // Get the root state machine
        AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;
        
        // Create states
        AnimatorState idleState = rootStateMachine.AddState("Idle");
        AnimatorState walkState = rootStateMachine.AddState("Walk");
        AnimatorState runState = rootStateMachine.AddState("Run");
        
        // Set state motions
        idleState.motion = idleMotion;
        walkState.motion = walkMotion;
        runState.motion = runMotion;
        
        // Set state positions for better organization
        // Note: We need to access through the state machine's states array
        var states = rootStateMachine.states;
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].state.name == "Idle")
                states[i].position = new Vector3(100, 100, 0);
            else if (states[i].state.name == "Walk")
                states[i].position = new Vector3(300, 100, 0);
            else if (states[i].state.name == "Run")
                states[i].position = new Vector3(500, 100, 0);
        }
        
        // Set entry state
        rootStateMachine.defaultState = idleState;
        
        // Create transitions
        // Idle -> Walk
        AnimatorStateTransition idleToWalk = idleState.AddTransition(walkState);
        idleToWalk.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        idleToWalk.duration = 0.1f;
        idleToWalk.hasExitTime = false;
        
        // Walk -> Idle
        AnimatorStateTransition walkToIdle = walkState.AddTransition(idleState);
        walkToIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        walkToIdle.duration = 0.1f;
        walkToIdle.hasExitTime = false;
        
        // Walk -> Run
        AnimatorStateTransition walkToRun = walkState.AddTransition(runState);
        walkToRun.AddCondition(AnimatorConditionMode.Greater, 0.7f, "Speed");
        walkToRun.duration = 0.1f;
        walkToRun.hasExitTime = false;
        
        // Run -> Walk
        AnimatorStateTransition runToWalk = runState.AddTransition(walkState);
        runToWalk.AddCondition(AnimatorConditionMode.Less, 0.7f, "Speed");
        runToWalk.duration = 0.1f;
        runToWalk.hasExitTime = false;
        
        // Run -> Idle
        AnimatorStateTransition runToIdle = runState.AddTransition(idleState);
        runToIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        runToIdle.duration = 0.2f;
        runToIdle.hasExitTime = false;
        
        // Save the animator controller
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Successfully created MasculineCharacterController at: " + ANIMATOR_PATH);
        Debug.Log("Parameters: Speed (float), IsMoving (bool)");
        Debug.Log("States: Idle, Walk, Run with automatic transitions based on Speed parameter");
    }
    
    private static Motion GetMotionFromController(AnimatorController controller, string stateName)
    {
        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.name == stateName)
                {
                    return state.state.motion;
                }
            }
        }
        return null;
    }
    
    void OnGUI()
    {
        GUILayout.Label("Character Animator Generator", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.Label("This tool creates a comprehensive animator controller");
        GUILayout.Label("with idle, walk, and run states for masculine characters.");
        GUILayout.Space(20);
        
        if (GUILayout.Button("Generate Animator Controller", GUILayout.Height(40)))
        {
            GenerateAnimatorController();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Output: Assets/Animators/MasculineCharacterController.controller");
        
        GUILayout.Space(20);
        GUILayout.Label("Parameters created:");
        GUILayout.Label("- Speed (float): Controls movement speed (0-1 range)");
        GUILayout.Label("- IsMoving (bool): Whether character is moving");
        
        GUILayout.Space(10);
        GUILayout.Label("States created:");
        GUILayout.Label("- Idle: Default state when Speed < 0.1");
        GUILayout.Label("- Walk: Active when 0.1 <= Speed < 0.7");
        GUILayout.Label("- Run: Active when Speed >= 0.7");
    }
}