using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleInputHandler : MonoBehaviour
{
    [Tooltip("The scene to go to after completing the puzzle")]
    public string nextSceneName = "EndScene";

    void Start()
    {
        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("✅ X pressed — loading end scene: " + nextSceneName);

            // Hide and lock the cursor again before leaving
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Load the next scene (EndScene)
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

/*
    example code to handle making player unable to see interaction messages, look around or move around while a puzzle pop up is active

    outside of a method write: 
    private PlayerInteraction playerInteraction; 
    private PlayerMovementAndCamera playerMovementAndCamera; 

    in awake or start write: 
    playerInteraction = FindFirstObjectByType<PlayerInteraction>(); if (PlayerInteraction==null) Debug.LogWarning("Player interaction null"); 
    playerMovementAndCamera = FindFirstObjectByType<PlayerMovementAndCamera>(); if (playerMovementAndCamera==null) Debug.LogWarning("Player movement and camera null");

    when you have the pop up active and want to make the player unable to see interaction messages, look around or move write this:
    playerInteraction.SetCanInteract(false);
    playerMovementAndCamera.SetCanMove(false); 
    playerMovementAndCamera.SetCanMoveCamera(false);

    when the player finishes the popup restore these values: 
    playerInteraction.SetCanInteract(true);
    playerMovementAndCamera.SetCanMove(true); 
    playerMovementAndCamera.SetCanMoveCamera(true);
    */