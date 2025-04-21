using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHints : MonoBehaviour
{
    //Inspired by interaction message. This is a very shoddy implementation but it will get the job done.
    private TextMeshProUGUI interactionTextField;
    private bool hint1Used;
    private bool hint2Used;
    private bool hint3Used;
    private bool hint4Used;
    // Start is called before the first frame update
    void Awake() { 
        var interactionTextGO = GameObject.Find("PlayerHints"); if (interactionTextGO == null) Debug.LogWarning("Player Hints not found");
        interactionTextField = interactionTextGO.GetComponent<TextMeshProUGUI>(); 
        hint1Used = false;
        hint2Used = false;
        hint3Used = false;
        hint4Used = false;
    }
    public void hint1() {
        if (!hint1Used) {
            interactionTextField.text = "Check the door";
            hint1Used = true;
        }
    }
    public void hint2() {
        if (!hint2Used) {
            interactionTextField.text = "Find a key";
            hint2Used = true;
        }
    }
    public void hint3() {
        if (!hint3Used) {
            interactionTextField.text = "Unlock the door";
            hint3Used = true;
        }
    }
    public void hint4() {
        if (!hint4Used) {
            interactionTextField.text = "Solve the puzzle";
            hint4Used = true;
        }
    }
}
