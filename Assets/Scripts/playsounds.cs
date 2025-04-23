using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playsounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioClip ac;
    private AudioSource chosen;
    private int s;

    private float timer = 0;
    public float minsoundPlayRate = 10;
    public float maxSoundPlayRate = 80;
    private float randomSoundTime;
    private float startCutsceneTime = 28;
    private bool isCutscenePlaying = true;
    public AudioClip[] cutsceneSounds;
    private bool[] cutscenesPlayed;
    // Start is called before the first frame update
    void Start()
    {
        cutscenesPlayed = new bool[cutsceneSounds.Length];
        for (int i=0; i<cutscenesPlayed.Length;i++) {
            cutscenesPlayed[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCutscenePlaying)
        {
            if (timer < startCutsceneTime)
            {
                if (timer > 2 && !cutscenesPlayed[0]) {
                    cutscenesPlayed[0] = true;
                    gameObject.GetComponent<AudioSource>().clip = cutsceneSounds[0];
                    GetComponent<AudioSource>().Play();
                }
                if (timer > 8 && !cutscenesPlayed[1]) {
                    cutscenesPlayed[1] = true;
                    gameObject.GetComponent<AudioSource>().clip = cutsceneSounds[1];
                    GetComponent<AudioSource>().Play();
                }
                if (timer > 14 && !cutscenesPlayed[2]) {
                    cutscenesPlayed[2] = true;
                    gameObject.GetComponent<AudioSource>().clip = cutsceneSounds[2];
                    GetComponent<AudioSource>().Play();
                }
                if (timer > 22 && !cutscenesPlayed[3]) {
                    cutscenesPlayed[3] = true;
                    gameObject.GetComponent<AudioSource>().clip = cutsceneSounds[3];
                    GetComponent<AudioSource>().Play();
                }
                timer += Time.deltaTime;
            }
            else
            {
                isCutscenePlaying = false;
                timer = 0;
            }
        }
        else
        {
            if (timer < minsoundPlayRate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                randomSoundTime = Random.Range(minsoundPlayRate, maxSoundPlayRate);

                if (timer >= randomSoundTime)
                {
                    s = Random.Range(0, sounds.Length - 1);
                    ac = sounds[s];

                    gameObject.GetComponent<AudioSource>().clip = ac;
                    chosen = GetComponent<AudioSource>();
                    chosen.Play();
                    timer = 0;
                }
            }
        }
    }
}
