using UnityEngine;

/// <summary>
/// Helper script to set up materials at runtime
/// </summary>
public class MaterialSetup : MonoBehaviour
{
    public Material highlightMaterial;
    
    void Awake()
    {
        // Create highlight material if it doesn't exist
        if (highlightMaterial == null)
        {
            Shader highlightShader = Shader.Find("Custom/ObjectHighlight");
            
            if (highlightShader != null)
            {
                highlightMaterial = new Material(highlightShader);
                highlightMaterial.name = "ObjectHighlight";
                
                // Set default properties
                highlightMaterial.SetColor("_HighlightColor", new Color(1f, 0.8f, 0f, 1f));
                highlightMaterial.SetFloat("_HighlightIntensity", 2f);
                highlightMaterial.SetFloat("_OutlineWidth", 0.02f);
                highlightMaterial.SetColor("_OutlineColor", new Color(0f, 0.8f, 1f, 1f));
                
                Debug.Log("Created highlight material at runtime");
            }
            else
            {
                Debug.LogError("ObjectHighlight shader not found!");
                // Fallback to standard shader
                highlightShader = Shader.Find("Standard");
                if (highlightShader != null)
                {
                    highlightMaterial = new Material(highlightShader);
                    highlightMaterial.name = "FallbackHighlight";
                    highlightMaterial.SetColor("_Color", new Color(1f, 0.8f, 0f, 1f));
                    Debug.LogWarning("Using fallback standard material for highlighting");
                }
            }
        }
    }
    
    /// <summary>
    /// Get the highlight material (create if needed)
    /// </summary>
    public static Material GetHighlightMaterial()
    {
        MaterialSetup instance = Object.FindAnyObjectByType<MaterialSetup>();
        
        if (instance != null)
        {
            return instance.highlightMaterial;
        }
        
        // Create a new instance if none exists
        GameObject setupObject = new GameObject("MaterialSetup");
        MaterialSetup setup = setupObject.AddComponent<MaterialSetup>();
        
        return setup.highlightMaterial;
    }
}