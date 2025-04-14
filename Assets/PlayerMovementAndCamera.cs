using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementAndCamera : MonoBehaviour
{
    [SerializeField] private float playerSpeed=5f;
    public float sensX; 
    public float sensY; 
    
    [SerializeField] private Rigidbody rb; 
    [SerializeField] private float groundDrag = 5f;
    
    private float horizontalInput; 
    private float verticalInput; 
    private float xRotation;
    private float yRotation; 
    private CinemachineVirtualCamera playerCamera; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        playerCamera = GetComponentInChildren<CinemachineVirtualCamera>(); 
        if (playerCamera == null) Debug.Log("Player camera null"); 
    }

    private void Update()
    {
        MyInput();
        rb.drag = groundDrag; 
    }

    public GameObject candle; //Ethan Change
    private void FixedUpdate()
    {
        Move(); 
        SmoothLook();
        candle.GetComponent<Candle>().funky(); //Ethan Change because it was tweaking with smooth look. move will be fine once i give it physics
    }

    void MyInput() { 
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical"); // -1 for back (S), 1 for forward (W) 

    }
    void Move() { 
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        rb.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
    }

    [SerializeField] private float smoothTime = 0.05f; 
    void SmoothLook(){
       float mouseX = Input.GetAxis("Mouse X") * sensX;
       float mouseY = Input.GetAxis("Mouse Y") * sensY;

       yRotation += mouseX;
       xRotation -= mouseY;

       xRotation = Mathf.Clamp(xRotation, -90f, 90f);

       var targetRotation = Quaternion.Euler(0, yRotation, 0);
       var targetCameraRotation = Quaternion.Euler(xRotation, yRotation, 0); 

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime);
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetCameraRotation, smoothTime);
    }


    public void SetPlayerCameraPriority(int priority) { 
        playerCamera.Priority = priority;         
    }

    public void DeactivateCamera() { 
        playerCamera.Priority = 0; 
    }

    public void ActivateCamera() { 
        playerCamera.Priority = 1; 
    }


}

