using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Holdable : MonoBehaviour
{
    // Holdable object script. Attach to holdable object scripts along with basic interaction scripts
    // and set the OnInteraction event to PickUpItem. 
    
    [SerializeField] private GameObject heldItemContainer; 
    private Rigidbody rb;
    private PlayerInteraction playerInteraction; 
    private Collider playerCollider; 
    private Collider thisCollider; 
    public bool isHeld; 
    [Header("Held object position variables")]
    [SerializeField] private bool hasCustomRotation; 
    [SerializeField] private bool hasCustomPosition; 
    [SerializeField] private Vector3 customLocalRotation; 
    [SerializeField] private Vector3 customLocalPosition; 
    [Header("Optional interact messages while held and not held")]
    [SerializeField] private string _while_not_heldInteractionMessage; 
    [SerializeField] private string _while_held_interaction_message; 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>(); 
        isHeld = false; 
        var player = GameObject.FindGameObjectWithTag("Player");
        playerInteraction = player.GetComponent<PlayerInteraction>(); 
        playerCollider = player.GetComponent<Collider>(); 
        if (heldItemContainer==null) Debug.LogError("There is no held item container set on " + transform.name + ", set it to left or right hand item container");
    }

    void Update()
    {
        // Check if player is dropping the object 
        CheckIfDropped(); 
    }

    void CheckIfDropped() { 
    // If player is currently holding this item and presses E or interact button again while not trying to interact 
    // with some other interactable object, drop this item 
        if (isHeld && Input.GetKeyDown(KeyCode.E) && playerInteraction.GetCurrentInteractable() == null) {
            Debug.Log("E called while looking at nothing; dropping object now"); 
            DropItem();  
        }
    }

    // Picks up item. Called by player, helped by basic interaction script. 
    public void PickUpItem() {
        if (playerInteraction.GetCanInteract() == false) return; 
        // Check if heldItemContainer already has a holdable child; if so, call drop object on this other held
        Debug.Log("Pick up item called on " + transform.name);
        
        Holdable currentlyHolding = heldItemContainer.GetComponentInChildren<Holdable>(); 
        if  (currentlyHolding != this && currentlyHolding != null) { 
            Debug.Log("Already holding something else; dropping item " + currentlyHolding.transform.name);
            currentlyHolding.DropItem(); 
        }
        else if (isHeld) { 
            // Player is looking at this object but is already holding it; in this case, drop the object 
            Debug.Log("This object already held, dropping item " + transform.name + " and returning"); 
            DropItem(); 
            return; 
        } 
        Debug.Log("Picking up object " + transform.name + " now"); 
        // Now, set this as a held object
        SetAsHeld(); 
        // Set to custom local offsets or rotations if wanted 
        SetCustomPositionsAndRotations(); 
        // Ignore collision so that object cannot push or exert force on player
        Physics.IgnoreCollision(playerCollider, thisCollider, true);  
        // Change interaction messages (optional)
        if (GetComponent<InteractionMessage>() != null) GetComponent<InteractionMessage>().ChangeInteractionMessage(_while_held_interaction_message); 
    }

    // Drops item, unparents it from held item container, turns kinematic on so gravity can pull it downwards
    public void DropItem() { 
        // If player cannot drop, then do not continue with drop. (One case: when player is hiding)
        if (!playerInteraction.GetCanDrop()) return; 
        Debug.Log("Drop item called on " + transform.name );
        SetAsNotHeld();
        // Restore collision 
        Physics.IgnoreCollision(playerCollider, thisCollider, true);  
        // Change interaction messages (optional) 
        if (GetComponent<InteractionMessage>() != null) GetComponent<InteractionMessage>().ChangeInteractionMessage(_while_not_heldInteractionMessage); 
    }

    void SetAsHeld() { 
        isHeld = true; 
        transform.parent = heldItemContainer.transform; 
        rb.isKinematic = true; 
    }

    void SetAsNotHeld() { 
        isHeld = false; 
        transform.parent = null; 
        rb.isKinematic = false; 
    }

    void SetCustomPositionsAndRotations() { 
        transform.localRotation = Quaternion.identity; 
        transform.localPosition = Vector3.zero; 
        if (hasCustomPosition) transform.localPosition = customLocalPosition;
        if (hasCustomRotation) transform.localEulerAngles = customLocalRotation;
    }
    

}
