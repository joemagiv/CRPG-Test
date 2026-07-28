# Top-Down Isometric Character & Camera Controllers

This package provides basic character and camera controllers for a top-down isometric adventure game in the style of Disco Elysium.

## Components

### 1. CharacterController
Located at: `Assets/Scripts/Character/CharacterController.cs`

**Features:**
- NavMesh-based movement
- Smooth rotation to face movement direction
- Animation parameter control (speed, isMoving)
- Click-to-move functionality

**Requirements:**
- GameObject must have `NavMeshAgent` and `Animator` components
- NavMesh baked in your scene

### 2. IsometricCameraController
Located at: `Assets/Scripts/Camera/IsometricCameraController.cs`

**Features:**
- Isometric camera positioning (45° angle by default)
- Smooth follow of target character
- Configurable height, distance, and angle
- Movement boundaries

### 3. ClickToMove
Located at: `Assets/Scripts/Input/ClickToMove.cs`

**Features:**
- Mouse click detection for movement
- Ground layer detection
- Obstacle avoidance
- UI click filtering

### 4. GameSetup
Located at: `Assets/Scripts/GameSetup.cs`

**Features:**
- Automatic scene configuration
- Character spawning
- Camera setup
- Input system initialization

## Setup Instructions

### 1. Prepare Your Scene

1. Open the `Demo.scene` from `Assets/Synty/PolygonSciFiCity/Scenes/`
2. Make sure you have a NavMesh baked for your level
3. Set up appropriate layers for ground and obstacles

### 2. Set Up Character

1. Drag a character prefab from `Assets/Synty/PolygonGeneric/Prefabs/Characters/` into your scene
2. Add the `CharacterController` script to your character
3. Configure the NavMeshAgent component:
   - Set appropriate speed
   - Adjust radius to match character size
   - Set stopping distance

### 3. Set Up Camera

1. Select your main camera
2. Add the `IsometricCameraController` script
3. Set the target to your character's transform
4. Adjust height, distance, and angle as needed

### 4. Set Up Input

1. Select your main camera
2. Add the `ClickToMove` script
3. Set the character controller reference
4. Configure ground and obstacle layer masks

### 5. Alternative: Use GameSetup (Recommended)

1. Create an empty GameObject called "GameManager"
2. Add the `GameSetup` script
3. Assign your character prefab and camera settings
4. The script will automatically configure everything

## Usage

### Basic Movement
```csharp
// Get character controller reference
CharacterController character = GetComponent<CharacterController>();

// Move to a position
character.MoveToPosition(new Vector3(10f, 0f, 5f));

// Stop movement
character.StopMovement();

// Check if moving
bool isMoving = character.IsMoving();
```

### Camera Control
```csharp
// Get camera controller reference
IsometricCameraController camera = GetComponent<IsometricCameraController>();

// Set target
camera.SetTarget(characterTransform);

// Adjust camera parameters
camera.SetHeight(12f);
camera.SetDistance(8f);
camera.SetRotationAngle(30f);
```

## Animation Setup

The character controller expects the following animation parameters:
- `Speed` (float): Movement speed (0-1 range)
- `IsMoving` (bool): Whether character is moving

Make sure your animator controller has these parameters and appropriate transitions.

## Troubleshooting

**Character doesn't move:**
- Check if NavMesh is baked
- Verify NavMeshAgent component is properly configured
- Ensure ground layer mask is correctly set in ClickToMove

**Camera doesn't follow:**
- Check if target is assigned in IsometricCameraController
- Verify camera is not obstructed by other scripts

**Click-to-move doesn't work:**
- Check layer masks in ClickToMove script
- Ensure no UI elements are blocking mouse clicks
- Verify ground layer has colliders

## Dependencies

- Unity Input System (for advanced input handling)
- NavMesh system (for pathfinding)
- Synty Polygon assets (for characters and environment)

## License

This code is provided as-is for your game development needs. Feel free to modify and extend as needed.