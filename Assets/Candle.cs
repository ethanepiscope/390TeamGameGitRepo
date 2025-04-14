using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    //use this to get rotation and movespeed
    public GameObject player;
    [SerializeField] float currentRotation;
    [SerializeField] float prevRotation;
    [SerializeField] float rotSpeed;
    [SerializeField] float xPos;
    [SerializeField] float yPos;
    [SerializeField] float zPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(xPos,yPos,zPos+3);
    }
    void Update()
    {
        float xDiff = player.transform.position.x-xPos;
        float yDiff = player.transform.position.y-yPos;
        float zDiff = player.transform.position.z-zPos;
        xPos = player.transform.position.x;
        yPos = player.transform.position.y;
        zPos = player.transform.position.z;
        transform.position = transform.position + new Vector3(xDiff,yDiff,zDiff);
    }
    // Update is called once per frame
    public void funky()
    {
        currentRotation = player.transform.rotation.eulerAngles.y;
        rotSpeed = (currentRotation - prevRotation)/Time.fixedDeltaTime;
        prevRotation = currentRotation;
        transform.RotateAround(player.transform.position,Vector3.up, rotSpeed*Time.fixedDeltaTime);
    }
}
