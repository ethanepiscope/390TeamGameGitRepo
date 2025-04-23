using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScreenController : MonoBehaviour
{
    public TMP_Text promptText;               // Drag in your "Press X" text
    public string gameSceneName = "house";   
    public float blinkSpeed = 0.75f;          // Optional: blinking text effect

    private float timer;

    void Update()
    {
        // Blink the text (optional)
        timer += Time.deltaTime;
        if (promptText != null)
        {
            promptText.alpha = Mathf.PingPong(timer * blinkSpeed, 1f);
        }

        // Load next scene on input
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("▶️ Loading scene: " + gameSceneName);
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
