using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BasicInteraction : IInteractable { 
    [SerializeField] private UnityEvent onHover; 
    [SerializeField] private UnityEvent onHoverExit; 
    [SerializeField] private UnityEvent onInteraction; 
    public void OnHover() { 
        onHover.Invoke();
    }

    public void OnHoverExit() { 
        onHoverExit.Invoke();
    }

    public void OnInteraction() { 
        onInteraction.Invoke(); 
    }

    public void DropItem() { 
        // Implement here 
    }

}