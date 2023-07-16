using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : MonoBehaviour {
    public  GameObject    textArea;
    public  GameObject    characterNameText;
    public  GameObject    characterTitleText;
    public  Image         background; 
    public  NarrationItem startingNarrativeItem;
    private NarrationItem _currentNarrativeItem;
    public  AudioSource   audioSource;
    private Coroutine     _audioCoroutine;
    private TMP_Text      _narrativeLineText;
    private AnimateInText _animateInText;
    private TMP_Text      _characterName;
    private TMP_Text      _characterTitle;

    private void Start() {
        _characterTitle = characterTitleText.GetComponent<TMP_Text>();
        _characterName = characterNameText.GetComponent<TMP_Text>();
        _animateInText = textArea.GetComponent<AnimateInText>();
        _narrativeLineText = textArea.GetComponent<TMP_Text>();
        _currentNarrativeItem = startingNarrativeItem;
        RunNarrativeItem();
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0) && _currentNarrativeItem.next.Count == 1 && _currentNarrativeItem.next[0].button == null) {
            AdvanceNarrative(0);
        }
    }

    public void AdvanceNarrative(int option = 0) {
        StopPreviousItem();
        _currentNarrativeItem = _currentNarrativeItem.next[0].narrativeItem;
        RunNarrativeItem();
    }

    private void StopPreviousItem() {
        if(_audioCoroutine != null) {
            StopCoroutine(_audioCoroutine);
        }        
        if(audioSource != null) {
            audioSource.Stop();
        }        
    }

    private void RunNarrativeItem() {
        if (_currentNarrativeItem == null) return;
        // update text area
        _narrativeLineText.text = _currentNarrativeItem.line;
        _animateInText.AnimateText();
        _characterName.text = _currentNarrativeItem.character.name;
        _characterTitle.text = _currentNarrativeItem.character.title;
        background.sprite = _currentNarrativeItem.background;
        // update background
        // update characters
        _audioCoroutine=StartCoroutine(PlayAudioClips());

        
    }
    
    private IEnumerator PlayAudioClips() {
        foreach (AudioClip clip in _currentNarrativeItem.sounds) {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }
    
}
