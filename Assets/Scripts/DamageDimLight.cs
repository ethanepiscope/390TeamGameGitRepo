using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;

public class DamageDimLight : MonoBehaviour
{
    private bool isDamaged = false;
    [Header ("Holdable Things")]
    public Candle candle;
    public Holdable hold;

    [Header ("Damage")]
    public Image overlay; //damage overlay image
    public float duration; //how long overlay stays on screen
    public float fadeSpeed; //how fast image fades
    private float durationTimer; //timer to check w/ duration
    // Start is called before the first frame update

    private AudioSource Audio;
    void Start()
    {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        Audio = GetComponent<AudioSource>();
        candle = GameObject.FindGameObjectWithTag("Candle").GetComponent<Candle>();
        hold = GameObject.FindGameObjectWithTag("Candle").GetComponent<Holdable>();
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

        // if (overlay.color.a > 0)
        // {
        //     Debug.Log("overlay.color.a = " + overlay.color.a);
        //     Debug.Log("duration timer: " + durationTimer);
        //     durationTimer += Time.deltaTime;
        //     if (durationTimer > duration)
        //     {
        //         Debug.Log("DurationTimer > Duration");
        //         float tempAlpha = overlay.color.a;
        //         Debug.Log("tempAlpha: " + tempAlpha);
        //         if (tempAlpha > 0)
        //         {
        //             tempAlpha -= Time.deltaTime * fadeSpeed;
        //             overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
        //             Debug.Log("tempAlpha > 0");
        //         }
        //     }
        // }
        // Debug.Log(overlay.color.a);
    }

    IEnumerator FadeOverTime() { 
        float alpha = 1; 
        while (overlay.color.a > 0) {
            if (alpha <= 0.2f) alpha = 0f;
            else alpha -= 0.2f; 
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, alpha);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    void damageIndic()
    {
        Debug.Log("Damage indic"); 
        if (hold.isHeld) candle.decreaseFlameMajor();
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        durationTimer = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is colliding with obstacle
        if (collision.gameObject.CompareTag("Player"))
        {
            isDamaged = true;
            Audio.Play();
            Debug.Log("being damaged");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the player is no longer colliding with obstacle
        // Debug.Log("OnCollisionExit");
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("OnCollisionExit with player");
            isDamaged = false;
            // Debug.Log("Is damaged: " + isDamaged);
            StartCoroutine(FadeOverTime()); 
        }
    }
}