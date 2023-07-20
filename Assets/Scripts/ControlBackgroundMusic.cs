using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBackgroundMusic : MonoBehaviour {
    public AudioSource     source;
    public List<BackgroundSong> backgroundSongs;
    void Start() {
        
    }

    public void PlaySong(Day day) {
        source.Stop();
        source.clip = backgroundSongs.Find((a) => a.day == day).song;
        source.Play();
    }
    
}

[Serializable]
public class BackgroundSong {
    [SerializeField]
    public Day       day;
    [SerializeField]
    public AudioClip song;
}
