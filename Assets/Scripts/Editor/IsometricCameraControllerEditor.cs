using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor for IsometricCameraController to make configuration easier
/// </summary>
[CustomEditor(typeof(IsometricCameraController))]
public class IsometricCameraControllerEditor : Editor
{
    private SerializedProperty targetProp;
    private SerializedProperty heightProp;
    private SerializedProperty distanceProp;
    private SerializedProperty rotationAngleProp;
    private SerializedProperty smoothSpeedProp;
    private SerializedProperty minPositionProp;
    private SerializedProperty maxPositionProp;
    
    void OnEnable()
    {
        targetProp = serializedObject.FindProperty("target");
        heightProp = serializedObject.FindProperty("height");
        distanceProp = serializedObject.FindProperty("distance");
        rotationAngleProp = serializedObject.FindProperty("rotationAngle");
        smoothSpeedProp = serializedObject.FindProperty("smoothSpeed");
        minPositionProp = serializedObject.FindProperty("minPosition");
        maxPositionProp = serializedObject.FindProperty("maxPosition");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(targetProp, new GUIContent("Target"));
        EditorGUILayout.PropertyField(heightProp, new GUIContent("Height"));
        EditorGUILayout.PropertyField(distanceProp, new GUIContent("Distance"));
        EditorGUILayout.PropertyField(rotationAngleProp, new GUIContent("Rotation Angle"));
        EditorGUILayout.PropertyField(smoothSpeedProp, new GUIContent("Smooth Speed"));
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Movement Boundaries", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(minPositionProp, new GUIContent("Min Position"));
        EditorGUILayout.PropertyField(maxPositionProp, new GUIContent("Max Position"));
        
        serializedObject.ApplyModifiedProperties();
        
        // Add helpful buttons
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Reset to Default Isometric View"))
        {
            ResetToDefaultIsometric();
        }
        
        if (GUILayout.Button("Set Target to Player"))
        {
            SetTargetToPlayer();
        }
    }
    
    private void ResetToDefaultIsometric()
    {
        IsometricCameraController controller = (IsometricCameraController)target;
        
        Undo.RecordObject(controller, "Reset Camera Settings");
        
        controller.height = 10f;
        controller.distance = 10f;
        controller.rotationAngle = 45f;
        controller.smoothSpeed = 5f;
        
        EditorUtility.SetDirty(controller);
        Debug.Log("Camera reset to default isometric settings");
    }
    
    private void SetTargetToPlayer()
    {
        IsometricCameraController controller = (IsometricCameraController)target;
        
        // Try to find a player object with IsometricCharacterController
        IsometricCharacterController character = Object.FindAnyObjectByType<IsometricCharacterController>();
        
        if (character != null)
        {
            Undo.RecordObject(controller, "Set Camera Target");
            controller.SetTarget(character.transform);
            EditorUtility.SetDirty(controller);
            Debug.Log("Camera target set to character: " + character.name);
        }
        else
        {
            Debug.LogWarning("No IsometricCharacterController found in scene");
        }
    }
}