using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    private PlayerMovementAndCamera playerMovementAndCamera;
    private PlayerInteraction playerInteraction; 
    [SerializeField] private CinemachineVirtualCamera hidingCamera;
    [SerializeField] private Renderer playerRenderer; 
    [SerializeField] private static float hideCooldown = 0.1f; 
    private float hideTimer = hideCooldown; 
    private bool isHiding;
    private InteractionMessage interactionMessage; 
    [SerializeField] private string _whileHidingInteractionMessage;
    [SerializeField] private string _whileNotHidingInteractionMessage;
    //Ethan adding here
    private ParticleSystem candleFlame;
    private Holdable candleHoldable;
    private GameObject candleModel;

    void Awake()
    {
        playerMovementAndCamera = GameObject.FindFirstObjectByType<PlayerMovementAndCamera>(); 
        if (playerMovementAndCamera==null) Debug.Log("Player camera not set"); 
        if (hidingCamera==null) Debug.LogWarning("Hiding camera is not set for object " + transform.name);
        playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>(); 
        interactionMessage = GetComponent<InteractionMessage>(); if (interactionMessage == null) Debug.LogWarning("Interaction message not found on " + transform.name + "; consider adding one."); 
        playerInteraction = GameObject.FindFirstObjectByType<PlayerInteraction>(); 
        if (playerInteraction == null) Debug.Log("Player interaction found null by object " + transform.name);
        //Ethan adding here:
        GameObject candle = GameObject.FindGameObjectWithTag("Candle");
        candleFlame = candle.GetComponent<ParticleSystem>();
        candleHoldable = candle.GetComponent<Holdable>();
        candleModel = candle.transform.Find("Candle_3dModel").gameObject;
    }

    void Update() => CheckStopHiding();
    public void Hide() {
        Debug.Log("Hiding called"); 
        if (isHiding) { 
            // Already hiding in this place
            StopHiding();
            return;
        }
        isHiding = true;
        hidingCamera.Priority = 1; 
        playerMovementAndCamera.DeactivateCamera();
        playerRenderer.enabled = false; 
        if (interactionMessage != null) interactionMessage.ChangeInteractionMessage(_whileHidingInteractionMessage);
        hideTimer = hideCooldown; // Reset to cooldown to prevent triggering hide and stop hiding in same E press 
        
        // Make sure player cannot move or drop objects (like candles) 
        playerMovementAndCamera.SetCanMove(false);
        playerMovementAndCamera.SetCanMoveCamera(false);
        playerInteraction.SetCanDrop(false); 
        playerInteraction.SetIsHiding(true);
        //Ethan adding code:
        if (candleHoldable.isHeld) {
            candleModel.SetActive(false);
            candleFlame.Stop();
        }
    }

    public void CheckStopHiding() { 
        if (isHiding) { 
            hideTimer -= Time.deltaTime; 
            // Short cooldown prevents hide and stop hiding from triggering in the same frame instantaneously
            if (Input.GetKeyDown(KeyCode.E) && hideTimer <= 0f) {
                Debug.Log("Stop hiding detected");
                StopHiding();
            }
        }
    }
    public void StopHiding() { 
        Debug.Log("StopHiding called");
        hidingCamera.Priority = 0;  
        playerMovementAndCamera.ActivateCamera();
        if (interactionMessage!= null) interactionMessage.ChangeInteractionMessage(_whileNotHidingInteractionMessage);
        StartCoroutine(DelayedReset());
    }

    IEnumerator DelayedReset() { 
        yield return new WaitForSeconds(0.2f); 
        isHiding = false;
        playerMovementAndCamera.SetCanMove(true); 
        playerMovementAndCamera.SetCanMoveCamera(true);
        playerInteraction.SetCanDrop(true);
        playerInteraction.SetIsHiding(false); 
        yield return new WaitForSeconds(0.6f);
        playerRenderer.enabled = true;
        candleModel.SetActive(true);
        candleFlame.Play();
    }
    public bool GetIsHiding() => isHiding; 
}
