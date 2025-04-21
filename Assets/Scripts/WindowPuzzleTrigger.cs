using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowPuzzleTrigger : MonoBehaviour
{
    private bool playerInRange = false;
    public GameObject fixPrompt; // UI popup

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("WindowPuzzleScene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (fixPrompt != null) fixPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (fixPrompt != null) fixPrompt.SetActive(false);
        }
    }
}
