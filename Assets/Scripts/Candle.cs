using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    //use this to get rotation and movespeed
    //public GameObject player;
    //[SerializeField] float currentRotation;
    //[SerializeField] float prevRotation;
    //[SerializeField] float rotSpeedDegrees;
    //[SerializeField] float rotSpeedMeters;
    //[SerializeField] float heldDistance = 3f;
    //[SerializeField] float tenUpdatesAgoRotation; //checking speed over longer interval to smooth things out
    [SerializeField] float xPrevPos;
    [SerializeField] float yPrevPos;
    [SerializeField] float zPrevPos;
    [SerializeField] int callCounterVelocity = 0;
    [SerializeField] int callCounterMajorDecrease = 0;
    ParticleSystem ps;
    ParticleSystem.MainModule main;
    float maxStartLifetime;
    float maxStartSize;
    public Light pointLight;
    float maxIntensity;
    bool decreaseFlameMajorCalled;
    GameOverLogic gameOverLogic;
    public GameObject wax;
    Material waxMaterial;
    Color originalWaxColor;
    private bool wet;
    private float velocity;
    public PlaySFX playsfx;
    private bool dead;
    

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
        maxStartLifetime = main.startLifetime.constant;
        maxStartSize = main.startSize.constant;
        maxIntensity = pointLight.intensity;
        decreaseFlameMajorCalled = false;
        wet = false;
        velocity = 0;
        dead = false;
        waxMaterial = wax.GetComponent<Renderer>().material;
        originalWaxColor = waxMaterial.GetColor("_EmissionColor");
        gameOverLogic = GameObject.FindGameObjectWithTag("GameOverManager").GetComponent<GameOverLogic>();
    }
    void FixedUpdate()
    {  
        if (wet) decreaseFlameMajor();
        Transform parent = transform.parent;
        if (callCounterVelocity == 0) {
            if (parent != null) {
                float xDiff = parent.position.x-xPrevPos;
                float yDiff = parent.position.y-yPrevPos;
                float zDiff = parent.position.z-zPrevPos;
                xPrevPos = parent.position.x;
                yPrevPos = parent.position.y;
                zPrevPos = parent.position.z;
                velocity = Mathf.Sqrt(xDiff*xDiff + yDiff*yDiff + zDiff*zDiff)/(10*Time.fixedDeltaTime);
                if (!decreaseFlameMajorCalled) {
                    if (velocity > 1) decreaseFlameMinor();
                    else increaseFlame();
                }
            }
            else if (!decreaseFlameMajorCalled) {
                increaseFlame();
            }
            decreaseFlameMajorCalled = false;
        }
        callCounterVelocity = (callCounterVelocity+1)%10;
        if (!dead && pointLight.intensity <= 0) {
            playsfx.PlayAudio();
            gameOverLogic.GameOver();
            dead = true;
        }
    }
    public void decreaseFlameMinor() {
        float currentLifetime = main.startLifetime.constant;
        float currentSize = main.startSize.constant;
        if (currentSize > 7*maxStartSize/10 | velocity > 3) {
            main.startLifetime = new ParticleSystem.MinMaxCurve(currentLifetime-maxStartLifetime/20);
            main.startSize = new ParticleSystem.MinMaxCurve(currentSize-maxStartSize/20);
            pointLight.intensity -= maxIntensity/20;
            waxMaterial.SetColor("_EmissionColor",waxMaterial.GetColor("_EmissionColor")-originalWaxColor/20);
        }
        else if (currentSize <= maxStartSize/2) increaseFlame();
    }
    public void decreaseFlameMajor() {
        decreaseFlameMajorCalled = true;
        if (callCounterMajorDecrease == 0) {
            float currentLifetime = main.startLifetime.constant;
            float currentSize = main.startSize.constant;
            if (currentSize > 0) {
                main.startLifetime = new ParticleSystem.MinMaxCurve(currentLifetime-maxStartLifetime/10);
                main.startSize = new ParticleSystem.MinMaxCurve(currentSize-maxStartSize/10);
                pointLight.intensity -= maxIntensity/10;
                waxMaterial.SetColor("_EmissionColor",waxMaterial.GetColor("_EmissionColor")-originalWaxColor/10);
            }
        }
        callCounterMajorDecrease = (callCounterMajorDecrease+1)%10;
    }
    public void increaseFlame() {
        float currentLifetime = main.startLifetime.constant;
        float currentSize = main.startSize.constant;
        if (currentSize < maxStartSize && currentSize > 0) {
            main.startLifetime = new ParticleSystem.MinMaxCurve(currentLifetime+maxStartLifetime/10);
            main.startSize = new ParticleSystem.MinMaxCurve(currentSize+maxStartSize/10);
            pointLight.intensity += maxIntensity/10;
            waxMaterial.SetColor("_EmissionColor",waxMaterial.GetColor("_EmissionColor")+originalWaxColor/10);
        }
    }
    public void updateCandleRotation()
    {
        /*
        currentRotation = player.transform.rotation.eulerAngles.y;
        if (callCounter==0) {
            rotSpeedDegrees = (currentRotation - tenUpdatesAgoRotation)/(10*Time.fixedDeltaTime);
            rotSpeedMeters = Mathf.PI * heldDistance * rotSpeedDegrees/180;
            //if i want directional, take orthonormal to currentRotation angle and multiply by this
            //positive to the right ig
            float movementDirection = (currentRotation+90)%360*Mathf.PI/180;
            //this is in radians
            //x=0,z=3 at 0
            //x=3,z=0 at 90
            //x=0,z=-3 at 180
            //x=-3,z=0 at 270
            //z horizontal, x vertical axis
            Vector3 velocity = rotSpeedMeters*new Vector3(Mathf.Sin(movementDirection),0,Mathf.Cos(movementDirection));
            //make sure to add different particl system settings corresponding to light levels
            Debug.Log(movementDirection);
            Debug.Log(velocity);
            tenUpdatesAgoRotation = currentRotation;
        }
        transform.RotateAround(player.transform.position,Vector3.up,currentRotation-prevRotation);
        prevRotation = currentRotation;
        callCounter = (callCounter+1)%10;
        */
    }
    void OnTriggerEnter(Collider other)
    {
      if (other.tag=="WaterHitbox") {
        wet = true;
      } 
    }

    void OnTriggerExit(Collider other)
    {
      if (other.tag=="WaterHitbox") {
        wet = false;
      } 
    }
    
}
