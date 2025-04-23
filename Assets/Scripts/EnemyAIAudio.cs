using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAIAudio : MonoBehaviour
{
    // Script methods are triggered by animation events (ex PlayRandomRageRoar event) or through enemy AI script
    private AudioSource audioSource; 
    [SerializeField] private AudioClip[] rageRoars; 
    [SerializeField] private AudioClip[] aggressiveGrowls; // Growls played during active pursuit or attack
    [SerializeField] private AudioClip[] passiveGrowls; // Growls played at some random intervals if player is nearby 
    [SerializeField] private AudioClip footsteps; 
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();     
    }

    public void PlayRandomRageRoar() { 
        int index = Random.Range(0, rageRoars.Length);
        PlayClip(rageRoars[index]);
    }

    public void PlayRandomAggressiveGrowl() { 
        int index = Random.Range(0, aggressiveGrowls.Length);
        PlayClip(aggressiveGrowls[index]);
    }

    public void PlayRandomPassiveGrowl() { 
        int index = Random.Range(0, passiveGrowls.Length);
        PlayClip(passiveGrowls[index]);
    }

    public void PlayFootsteps() { 
        PlayClip(footsteps);
    }
    void PlayClip(AudioClip clip) { 
        audioSource.clip = clip;
        audioSource.Play(); 
    }
    
}
