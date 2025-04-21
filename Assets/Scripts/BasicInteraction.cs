using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BasicInteraction : MonoBehaviour, IInteractable { 
    [SerializeField] private UnityEvent onHover; 
    [SerializeField] private UnityEvent onHoverExit; 
    [SerializeField] private UnityEvent onInteraction; 

    public void OnHover() { 
        // Debug.Log("On hover called on " + transform.name);
        onHover.Invoke();
    }

    public void OnHoverExit() { 
        // Debug.Log("On hover exit called on " + transform.name);
        onHoverExit.Invoke();
    }

    public void OnInteraction() { 
        Debug.Log("On interaction called on " + transform.name);
        onInteraction.Invoke(); 
    }

}