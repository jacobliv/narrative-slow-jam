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
    public  GameObject    phoneUi;
    public  GameObject    spokenTextUi;
    public  TMP_Text      phoneName;
    public  TMP_Text      phoneText;
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
            AdvanceNarrative();
        }
    }

    public void AdvanceNarrative(int option = 0) {
        StopPreviousItem();
        spokenTextUi.SetActive(true);

        OpenPhone(option);
        
        _currentNarrativeItem = _currentNarrativeItem.next[option].narrativeItem;
        RunNarrativeItem();
    }

    private void OpenPhone(int option) {
        if (!_currentNarrativeItem.next[option].narrativeItem.phone || _currentNarrativeItem.phone) {
            phoneUi.SetActive(false);
            return;
        }
        StartCoroutine(PhoneOpeningSequence());
    }

    private IEnumerator PhoneOpeningSequence() {
        spokenTextUi.SetActive(false);
        //Play phone ding and display notification icon on screen
        //wait
        //Open phone and play sfx sound
        phoneUi.SetActive(true);
        
        yield break;
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
        UpdateSpokenText();
        UpdatePhoneText();

        // update background
        background.sprite = _currentNarrativeItem.background;

        // update characters
        _audioCoroutine=StartCoroutine(PlayAudioClips());

        
    }

    private void UpdatePhoneText() {
        if(!_currentNarrativeItem.phone) return;
        phoneText.text = _currentNarrativeItem.line;
        phoneName.text = _currentNarrativeItem.character.name;

    }

    private void UpdateSpokenText() {
        if(_currentNarrativeItem.phone) return;
        _narrativeLineText.text = _currentNarrativeItem.line;
        _animateInText.AnimateText();
        _characterName.text = _currentNarrativeItem.character.name;
        _characterTitle.text = _currentNarrativeItem.character.title;
    }

    private IEnumerator PlayAudioClips() {
        foreach (AudioClip clip in _currentNarrativeItem.sounds) {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }
    
}
