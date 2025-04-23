using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject key; 
    [SerializeField] private GameObject leftHeldItemContainer; 
    [SerializeField] private GameObject fadeToWhite;
    private bool hasEscaped = false;
  

    void Awake()
    {
        if (key==null) Debug.LogWarning("Key is null");
        if (leftHeldItemContainer==null) Debug.LogWarning("Left held item container is null"); 
    }

    public void TryToEscape() { 
        // Test if user has the key needed 
        Debug.Log("TrytoEscape called");
        if (leftHeldItemContainer.transform.Find(key.name) && !hasEscaped) { 
            Escape(); 
        }
        
    }

    void Escape() { 
        Debug.Log("Successfully escaped!");
        hasEscaped = true; 
        StartCoroutine(EndScene()); 
    }

    IEnumerator EndScene() { 
        fadeToWhite.SetActive(true); 
        yield return new WaitForSeconds(1f);
        // Then, show  the end scene (to be shown)
        yield return null; 
    }
}
