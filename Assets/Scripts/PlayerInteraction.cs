using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] private IInteractable currentInteractable=null; 
    [SerializeField] private float playerReach;
    private bool _canDrop = true; 
    private CinemachineVirtualCamera playerCamera;
    private bool isHiding=false;
    private bool _canInteract = true;

    void Awake()
    {
       playerCamera = GetComponentInChildren<CinemachineVirtualCamera>();
       if (playerCamera == null) Debug.Log("Player camera is null"); 
    }

    // Update is called once per frame
    void Update() {
        UpdateCurrentInteractable(); 
        CheckInteraction(); 
        DebugDraw(); 
    }

    void DebugDraw() { 
        // Draws the interaction ray in the scene view 
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * playerReach, Color.cyan);
    }

    void UpdateCurrentInteractable() { 
        // Raycasts from the Player Camera (main camera)
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
        out hit, playerReach)) {
            //Debug.Log("Ray hit: " + hit.transform.name);
            // Checks if hit is an interactable object (implements IInteractable)
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            
            if (interactable != null) {
                //Call interactable's OnHover method 
                interactable.OnHover(); 
                if (currentInteractable != null && currentInteractable != interactable) currentInteractable.OnHoverExit(); 
                currentInteractable = interactable; 
                // Debug.Log("Detected interactable and current interactable set"); 
            }
            // If hit is not interactable, then
            else { 
                // If applicable, clear currInteractable and call its OnHoverExit 
                if (currentInteractable != null) currentInteractable.OnHoverExit();
                currentInteractable=null; 
            }
        }
        else { // If no collider was hit by the raycast, 
            // If applicable, clear currInteractable and call its OnHoverExit 
            if (currentInteractable != null) currentInteractable.OnHoverExit(); 
            currentInteractable=null; 
        }
    }
    
    void CheckInteraction() { 
    //Checks if currentInteractable is != null and if player hits interact key (E)
        bool interactPressed = Input.GetKeyDown(KeyCode.E);  
        if (interactPressed && currentInteractable != null) { 
            currentInteractable.OnInteraction(); 
        }
    }

    public IInteractable GetCurrentInteractable() { 
        return currentInteractable; 
    }

    public void SetCanDrop(bool set) { 
        _canDrop = set; 
    }

    public bool GetCanDrop() => _canDrop; 

    public bool GetIsHiding() => isHiding; 

    public void SetIsHiding(bool set) => isHiding=set; 
    
    public bool GetCanInteract() => _canInteract; 

    public void SetCanInteract(bool set) => _canInteract = set;
}
