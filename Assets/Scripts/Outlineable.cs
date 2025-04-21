using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class Outlineable : MonoBehaviour
{
    // Script should be attached to objects we want to outline 
    // Typically used for hover interactions (when player looks at an outlineable object, its outline is enabled; when they look away, disabled for ex)
    private Outline outline;

    void Awake() => outline = GetComponent<Outline>(); 
    public void EnableOutline() {
        outline.enabled = true;
    }

    public void DisableOutline() { 
        outline.enabled = false;
    }
}
