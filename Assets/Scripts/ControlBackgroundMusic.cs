using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBackgroundMusic : MonoBehaviour {
    public AudioSource source;
    public AudioClip   beginning;
    public AudioClip   ending;
    public void ChangeToStartSong() {
        source.Stop();
        source.clip = beginning;
        source.Play();
    }

    public void ChangeToEndSong() {
        source.Stop();
        source.clip = ending;
        source.Play();
    }

    
}
