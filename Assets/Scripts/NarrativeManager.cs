using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : MonoBehaviour {
    public  GameObject    textArea;
    public  NarrationItem startingNarrativeItem;
    private NarrationItem currentNarrativeItem;
    public AudioSource   audioSource;
    private Coroutine     audioCoroutine;

    private void Start() {
        currentNarrativeItem = startingNarrativeItem;
        RunNarrativeItem();
    }

    public void AdvanceNarrative(NarrationItem next, int option = 0) {
        StopPreviousItem();
        currentNarrativeItem = next;
        RunNarrativeItem();
    }

    private void StopPreviousItem() {
        StopCoroutine(audioCoroutine);
        audioSource.Stop();    
    }

    private void RunNarrativeItem() {
        if (currentNarrativeItem == null) return;
        // update text area
        // update background
        // update characters
        audioCoroutine=StartCoroutine(PlayAudioClips());

        
    }
    
    private IEnumerator PlayAudioClips() {
        foreach (AudioClip clip in currentNarrativeItem.sounds) {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }
    
}
