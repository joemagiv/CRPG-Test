using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor for IsometricCharacterController to make configuration easier
/// </summary>
[CustomEditor(typeof(IsometricCharacterController))]
public class IsometricCharacterControllerEditor : Editor
{
    private SerializedProperty moveSpeed;
    private SerializedProperty rotationSpeed;
    private SerializedProperty stoppingDistance;
    private SerializedProperty speedParameter;
    private SerializedProperty isMovingParameter;
    
    void OnEnable()
    {
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        stoppingDistance = serializedObject.FindProperty("stoppingDistance");
        speedParameter = serializedObject.FindProperty("speedParameter");
        isMovingParameter = serializedObject.FindProperty("isMovingParameter");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Move Speed"));
        EditorGUILayout.PropertyField(rotationSpeed, new GUIContent("Rotation Speed"));
        EditorGUILayout.PropertyField(stoppingDistance, new GUIContent("Stopping Distance"));
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animation Parameters", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(speedParameter, new GUIContent("Speed Parameter"));
        EditorGUILayout.PropertyField(isMovingParameter, new GUIContent("Is Moving Parameter"));
        
        serializedObject.ApplyModifiedProperties();
        
        // Add helpful buttons
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Configure NavMeshAgent"))
        {
            ConfigureNavMeshAgent();
        }
        
        if (GUILayout.Button("Test Movement"))
        {
            TestCharacterMovement();
        }
    }
    
    private void ConfigureNavMeshAgent()
    {
        IsometricCharacterController controller = (IsometricCharacterController)target;
        UnityEngine.AI.NavMeshAgent agent = controller.GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        if (agent != null)
        {
            Undo.RecordObject(agent, "Configure NavMeshAgent");
            
            agent.speed = controller.moveSpeed;
            agent.stoppingDistance = controller.stoppingDistance;
            agent.autoBraking = true;
            
            EditorUtility.SetDirty(agent);
            Debug.Log("NavMeshAgent configured to match IsometricCharacterController settings");
        }
    }
    
    private void TestCharacterMovement()
    {
        IsometricCharacterController controller = (IsometricCharacterController)target;
        
        // Move character 5 units forward
        Vector3 testPosition = controller.transform.position + controller.transform.forward * 5f;
        controller.MoveToPosition(testPosition);
        
        Debug.Log("Testing character movement to: " + testPosition);
    }
}