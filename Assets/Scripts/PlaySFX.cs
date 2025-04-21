using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    // Simple script to play SFX; just assign audio source in inspector. Can be used as a unity event for basic interaction. 
    [SerializeField] private AudioSource audioSource; 
    public void PlayAudio() {
        audioSource.Play();
    }

    public void StopAudio() {
        audioSource.Stop();
    }

    public void ChangeAudioClip(AudioClip clip) {  
        audioSource.clip = clip;
    }
}
