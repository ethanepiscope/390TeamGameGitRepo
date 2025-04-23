using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInvoker : MonoBehaviour
{
    private Dialogue dialogue;
    public int dialogue_hintIndex;
    public int hintClearIndex;
    public int delay = 3;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<Dialogue>();
    }

    public void callShowDialogue() {
        dialogue.ShowDialogue(dialogue_hintIndex,delay);
    }

    public void callShowHint() {
        dialogue.ShowHint(dialogue_hintIndex,delay);
    }
    public void callClearHint() {
        dialogue.ClearHint(hintClearIndex);
    }
}
