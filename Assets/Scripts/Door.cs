using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent(typeof(InteractionMessage))]
public class Door : MonoBehaviour
{
    // Start is called before the first frame update
   
    [SerializeField] private bool isLocked; 
    [SerializeField] private bool isOpen; // Set as false for closedDoorVersion, true for openDoorVersion
    [SerializeField] private GameObject key; 
    [SerializeField] private GameObject leftHeldItemContainer; 
    [SerializeField] private InteractionMessage interactionMessage; 
    [SerializeField] private GameObject closedDoorVersion;
    [SerializeField] private GameObject openDoorVersion; 
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip closeDoorClip; 
    [SerializeField] private AudioClip openDoorClip; 

    void Awake()
    {
        leftHeldItemContainer = GameObject.Find("LeftHandHeldItemContainer");
        if (leftHeldItemContainer == null) Debug.LogWarning("Left held item container is null on item " + transform.name); 
        interactionMessage = GetComponent<InteractionMessage>(); 
    }

    void Update()
    {
        // interactCooldownTimer -= Time.deltaTime; 
    }
    // Called when pressing E while looking at a door.
    public void TryOpen() { 
        Debug.Log("Try open called"); 
        if (isLocked && !isOpen) {
            // Locked and closed door; user needs to be holding specific key in their left hand 
            Debug.Log("Detected locked and closed door"); 
            if (leftHeldItemContainer.transform.Find(key.transform.name)) { 
                // Found key; can open
                Debug.Log("Found key match, opening door"); 
                OpenDoor();
            }
        }
        // If open, close the door. 
        else if (isOpen) { 
            Debug.Log("Door already open; closing door"); 
            CloseDoor(); 
        } 
        else if (!isLocked & !isOpen) { 
            Debug.Log("Opening unlocked closed door"); 
            OpenDoor();
        } 
    }
    // TODO: Make door visually open and close. For demo, probably just make it set to an open Transform vs a closed transform 
    public void OpenDoor() { 
        audioSource.clip = openDoorClip; 
        audioSource.Play(); 
        Debug.Log("Door opened"); 
        // if (interactCooldownTimer > 0f) return; 
        // isOpen = true; 
        openDoorVersion.SetActive(true); 
        closedDoorVersion.SetActive(false); 
        
    }

    public void CloseDoor() { 
        audioSource.clip = closeDoorClip; 
        audioSource.Play();  
        Debug.Log("Door closed"); 
 
        // isOpen = false; 
        openDoorVersion.SetActive(false); 
        closedDoorVersion.SetActive(true);
        // interactCooldownTimer = interactCooldown; 
    }

    // Called for on Hover with this door 
    public void VerifyMessage() { 
        bool hasKey = leftHeldItemContainer.transform.Find(key.transform.name);
        
        if (isLocked && !isOpen && hasKey) { 
            // Has key; interaction message should say "Open" 
            // Debug.Log("Changing interaction message to Open (E)");
            interactionMessage.ChangeInteractionMessage("Open (E)");
        }
        else if (isLocked && !isOpen && !hasKey) { 
            // Debug.Log("Changing interaction message to Door (locked). Needs key.");
            interactionMessage.ChangeInteractionMessage("Door (locked). Needs key.");
        }
    }

}
