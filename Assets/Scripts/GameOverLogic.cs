using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLogic : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject player;
    private PlayerMovementAndCamera pmac;

    // Start is called before the first frame update
    void Start()
    {
        pmac = player.GetComponent<PlayerMovementAndCamera>();
    }

    public void restartGame() {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pmac.enabled = true;
    }

    public void gameOver() {
        Debug.Log("Game over");
        gameOverScreen.SetActive(true);
        pmac.enabled = false;
    }

}
