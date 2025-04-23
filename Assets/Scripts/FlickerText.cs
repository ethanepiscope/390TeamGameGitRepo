using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Only if you're using TextMeshPro

public class FlickerText : MonoBehaviour
{
    public float flickerInterval = 0.5f; // How fast it blinks
    private float timer;

    private bool visible = true;
    private TextMeshProUGUI tmpText;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        timer = flickerInterval;
    }

    void Update()
    {
        // Flicker the text
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            visible = !visible;
            tmpText.enabled = visible;
            timer = flickerInterval;
        }

        // Listen for input
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
