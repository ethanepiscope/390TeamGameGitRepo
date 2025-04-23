using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    private int totalShards = 13;
    private int snappedCount = 0;
    private bool puzzleSolved = false;

    public GameObject solvedText; // This is the "Puzzle Solved! Press X..." UI Text
    
    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Start()
    {
        if (solvedText != null) solvedText.SetActive(false);
    }

    public void NotifyShardSnapped()
    {
        snappedCount++;

        if (snappedCount >= totalShards && !puzzleSolved)
        {
            puzzleSolved = true;
            Debug.Log("Puzzle complete!");
            if (solvedText != null) solvedText.SetActive(true);
        }
    }

    void Update()
    {
        if (puzzleSolved && Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
