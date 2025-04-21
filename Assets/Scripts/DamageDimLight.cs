using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDimLight : MonoBehaviour
{
    private bool isDamaged = false;
    public Candle candle;
    // Start is called before the first frame update
    void Start()
    {
        candle = GameObject.FindGameObjectWithTag("Candle").GetComponent<Candle>();
    }

    // Update is called once per frame
    // Ethan: changed this to fixed update since candle is doing fixed updates
    //        and I want them to be doing things at the same rate
    void FixedUpdate()
    {
        if (isDamaged)
        {
            damageIndic();
        }
    }

    void damageIndic()
    {
        candle.decreaseFlameMajor();
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is colliding with obstacle
        if (collision.gameObject.CompareTag("Player"))
        {
            isDamaged = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the player is no longer colliding with obstacle
        if (collision.gameObject.CompareTag("Player"))
        {
            isDamaged = false;
        }
    }
}