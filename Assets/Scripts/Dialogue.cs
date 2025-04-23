using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class Dialogue : MonoBehaviour
{
    public GameObject enemyAI;
    private float timer;
    private float startCutsceneTime;
    private bool isCutscenePlaying;
    public TextMeshProUGUI dialogueBox;
    public TextMeshProUGUI hintBox;
    public string[] lines;
    public string[] hints;
    public float textSpeed;
    private int index;
    private bool[] dialogueShown;
    private bool[] hintShown;
    private bool[] hintCleared;
    private bool currentlyTyping;
    private List<string> currentHints;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI.SetActive(false);
        timer = 0;
        startCutsceneTime = 28;
        isCutscenePlaying = true;
        dialogueBox.text = string.Empty;
        hintBox.text = string.Empty;
        dialogueShown = new bool[lines.Length];
        hintShown = new bool[hints.Length];
        hintCleared = new bool[hints.Length];
        for (int i=0; i<dialogueShown.Length;i++) {
            dialogueShown[i] = false;
            hintShown[i] = false;
            hintCleared[i] = false;
        }
        currentlyTyping = false;
        currentHints = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCutscenePlaying) {
            if (timer > startCutsceneTime) {
                enemyAI.SetActive(true);
                isCutscenePlaying = false;
            }
            else if (timer > 21.8) {
                ShowDialogue(5,1);
                ShowHint(5,5);
            }
            else if (timer > 20) {
                ShowDialogue(4,1);
            }
            else if (timer > 17.5) {
                ShowDialogue(3,1);
            }
            else if (timer > 15) {
                ShowDialogue(2,1);
            }
            else if (timer > 10) {
                ShowDialogue(1);
            }
            else if (timer > 5) {
                ShowDialogue(0);
            }
        }
        if (timer > 60) {
            ShowDialogue(9);
            ShowHint(9);
        }
        timer += Time.deltaTime;
    }
    
    public void ShowDialogue(int i, int delay=3) {
        if ((!isCutscenePlaying || i <= 5) && !dialogueShown[i] && !currentlyTyping) {
            index = i;
            dialogueShown[i] = true;
            currentlyTyping = true;
            StartCoroutine(TypeLineDialogue());
            StartCoroutine(ClearLineDialogue(delay));
        }
        
    }

    IEnumerator TypeLineDialogue() {
        foreach (char c in lines[index].ToCharArray()) {
            dialogueBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator ClearLineDialogue(int delay) {
        yield return new WaitForSeconds(lines[index].Length * textSpeed + delay);
        dialogueBox.text = string.Empty;
        currentlyTyping = false;
    }

    public void ShowHint(int i, int delay=3) {
        if (!hintShown[i]) {
            hintShown[i] = true;
            StartCoroutine(WriteHint(i,delay));
        }
    }

    IEnumerator WriteHint(int i, int delay) {
        yield return new WaitForSeconds(lines[i].Length * textSpeed + delay);
        currentHints.Add(hints[i]);
        hintBox.text = String.Join('\n',currentHints);
    }

    public void ClearHint(int i) {
        if (hintShown[i] && !hintCleared[i]) {
            hintCleared[i] = true;
            bool ret = currentHints.Remove(hints[i]);
            Debug.Log(ret);
            hintBox.text = String.Join('\n',currentHints);
        }
    }
}