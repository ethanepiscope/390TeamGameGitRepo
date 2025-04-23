using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class InteractionMessage : MonoBehaviour
{
    // Attach this script to gameobjects that have an interact message you want to display.
    // Common use case: Combine with basic interaction, assign interactionMessage in inspector, and have ShowTextField 
    // as OnHover, and clearTextField as an OnHoverExit method 
    [SerializeField] private string interactionMessage; 
    private TextMeshProUGUI interactionTextField; 
    private PlayerInteraction playerInteraction;
    // Finds the text field 
    void Awake() { 
        var interactionTextGO = GameObject.Find("InteractionMessage"); if (interactionTextGO == null) Debug.LogWarning("Interaction message not found");
        interactionTextField = interactionTextGO.GetComponent<TextMeshProUGUI>(); 
        playerInteraction = FindFirstObjectByType<PlayerInteraction>(); if (playerInteraction==null) Debug.LogWarning("Player interaction null from interaction message");
    }

    public void ShowTextField() { 
        if (playerInteraction.GetCanInteract() == false) return; 
        interactionTextField.text = interactionMessage; 
    }

    public void ClearTextField() { 
        interactionTextField.text = ""; 
    }

    public void ChangeInteractionMessage(string new_message) { 
        interactionMessage = new_message; 
    }
}
