using UnityEditor;
using UnityEngine;

/// <summary>
/// Helper script to fix common scene setup issues
/// </summary>
public class SceneSetupHelper : EditorWindow
{
    [MenuItem("Tools/Scene Setup Helper")]
    public static void ShowWindow()
    {
        GetWindow<SceneSetupHelper>("Scene Setup Helper");
    }
    
    [MenuItem("Tools/Scene Setup Helper/Fix Negative Collider Scaling")]
    public static void FixNegativeColliderScaling()
    {
        // Find all BoxColliders in the scene
        BoxCollider[] colliders = Object.FindObjectsByType<BoxCollider>(FindObjectsInactive.Include);
        int fixedCount = 0;
        
        foreach (BoxCollider collider in colliders)
        {
            // Check if any scale component is negative
            Vector3 scale = collider.transform.lossyScale;
            if (scale.x < 0 || scale.y < 0 || scale.z < 0)
            {
                // Fix negative scaling by making it positive
                Vector3 localScale = collider.transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);
                localScale.y = Mathf.Abs(localScale.y);
                localScale.z = Mathf.Abs(localScale.z);
                collider.transform.localScale = localScale;
                
                fixedCount++;
                Debug.Log("Fixed negative scaling on: " + collider.gameObject.name);
            }
        }
        
        if (fixedCount > 0)
        {
            Debug.Log("Fixed negative scaling on " + fixedCount + " colliders.");
            EditorUtility.SetDirty(Selection.activeGameObject);
        }
        else
        {
            Debug.Log("No colliders with negative scaling found.");
        }
    }
    
    [MenuItem("Tools/Scene Setup Helper/NavMesh Baking Instructions")]
    public static void ShowNavMeshBakingInstructions()
    {
        EditorUtility.DisplayDialog("NavMesh Baking Instructions", 
            "To bake NavMesh for your scene:\n\n" +
            "1. Open Window > AI > Navigation\n" +
            "2. Select the 'Bake' tab\n" +
            "3. Adjust bake settings if needed\n" +
            "4. Click 'Bake' button\n" +
            "5. Make sure your ground objects have 'Navigation Static' checked\n" +
            "6. Assign appropriate Navigation Areas to your objects\n\n" +
            "For the Demo scene, you may need to:\n" +
            "- Mark ground planes as Navigation Static\n" +
            "- Set obstacles as Navigation Static\n" +
            "- Bake the NavMesh before testing character movement",
            "OK");
    }
    
    void OnGUI()
    {
        GUILayout.Label("Scene Setup Helper", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        if (GUILayout.Button("Fix Negative Collider Scaling", GUILayout.Height(40)))
        {
            FixNegativeColliderScaling();
        }
        
        if (GUILayout.Button("NavMesh Baking Instructions", GUILayout.Height(40)))
        {
            ShowNavMeshBakingInstructions();
        }
        
        GUILayout.Space(20);
        GUILayout.Label("Common Issues:");
        GUILayout.Label("- NavMesh not baked: Character won't move");
        GUILayout.Label("- Negative scaling: Collider warnings");
        GUILayout.Label("- Input System: Use new Input System API");
    }
}