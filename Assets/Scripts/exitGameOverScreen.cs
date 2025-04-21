using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitGameOverScreen : MonoBehaviour
{
    GameOverLogic gameOverLogic;
    // Start is called before the first frame update
    void Start()
    {
        gameOverLogic = GameObject.FindGameObjectWithTag("GameOverManager").GetComponent<GameOverLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            gameOverLogic.restartGame();
        }
    }
}
