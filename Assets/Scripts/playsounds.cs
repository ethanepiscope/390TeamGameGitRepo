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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
