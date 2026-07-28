using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game setup script to configure the scene with character and camera
/// </summary>
public class GameSetup : MonoBehaviour
{
    [Header("Character Setup")]
    public GameObject characterPrefab;
    public Vector3 characterSpawnPosition = new Vector3(0f, 0f, 0f);
    
    [Header("Camera Setup")]
    public GameObject cameraPrefab;
    public float cameraHeight = 10f;
    public float cameraDistance = 10f;
    public float cameraAngle = 45f;
    
    [Header("Input Setup")]
    public LayerMask groundLayerMask = 1 << 0; // Default layer
    public LayerMask obstacleLayerMask = 1 << 0; // Default layer
    
    private IsometricCharacterController characterController;
    private IsometricCameraController cameraController;
    private ClickToMove clickToMove;
    
    void Start()
    {
        SetupGame();
    }
    
    /// <summary>
    /// Set up the game scene with character, camera, and input
    /// </summary>
    private void SetupGame()
    {
        // Spawn character
        if (characterPrefab != null)
        {
            GameObject character = Instantiate(characterPrefab, characterSpawnPosition, Quaternion.identity);
            characterController = character.GetComponent<IsometricCharacterController>();
            
            if (characterController == null)
            {
                characterController = character.AddComponent<IsometricCharacterController>();
            }
        }
        
        // Set up camera
        if (cameraPrefab != null)
        {
            GameObject cameraObj = Instantiate(cameraPrefab);
            cameraController = cameraObj.GetComponent<IsometricCameraController>();
            
            if (cameraController == null)
            {
                cameraController = cameraObj.AddComponent<IsometricCameraController>();
            }
            
            // Configure camera
            cameraController.height = cameraHeight;
            cameraController.distance = cameraDistance;
            cameraController.rotationAngle = cameraAngle;
            
            if (characterController != null)
            {
                cameraController.SetTarget(characterController.transform);
            }
        }
        else
        {
            // Use main camera if no prefab provided
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraController = mainCamera.GetComponent<IsometricCameraController>();
                if (cameraController == null)
                {
                    cameraController = mainCamera.gameObject.AddComponent<IsometricCameraController>();
                }
                
                cameraController.height = cameraHeight;
                cameraController.distance = cameraDistance;
                cameraController.rotationAngle = cameraAngle;
                
                if (characterController != null)
                {
                    cameraController.SetTarget(characterController.transform);
                }
            }
        }
        
        // Set up click-to-move input
        if (Camera.main != null)
        {
            clickToMove = Camera.main.GetComponent<ClickToMove>();
            if (clickToMove == null)
            {
                clickToMove = Camera.main.gameObject.AddComponent<ClickToMove>();
            }
            
            clickToMove.groundLayerMask = groundLayerMask;
            clickToMove.obstacleLayerMask = obstacleLayerMask;
            
            if (characterController != null)
            {
                clickToMove.SetCharacterController(characterController);
            }
        }
    }
    
    /// <summary>
    /// Get the character controller instance
    /// </summary>
    /// <returns>Character controller instance</returns>
    public IsometricCharacterController GetCharacterController()
    {
        return characterController;
    }
    
    /// <summary>
    /// Get the camera controller instance
    /// </summary>
    /// <returns>Camera controller instance</returns>
    public IsometricCameraController GetCameraController()
    {
        return cameraController;
    }
    
    /// <summary>
    /// Get the click-to-move input controller
    /// </summary>
    /// <returns>Click-to-move controller instance</returns>
    public ClickToMove GetClickToMoveController()
    {
        return clickToMove;
    }
}