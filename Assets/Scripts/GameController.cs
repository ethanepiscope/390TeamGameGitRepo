using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Manages game state 
      public enum GameState { 
        Waiting,
        Playing,
        GameOver
    }

    public GameState currentGameState; 

    void Awake() {
        MakeCursorLockedAndInvisible(); 
    }

    // Call this method to avoid mouse showing and lock it to the center of the screen
    public void MakeCursorLockedAndInvisible(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Should call this method for main menu / anytime we need player to interact with clickable buttons.
    public void MakeCursorVisible() { 
        Cursor.visible = true; 
    }

  
    


}
