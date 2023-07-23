using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : MonoBehaviour {
    #region Dialogue
    [Header("Dialogue")]
    public  GameObject      dialogueArea;
    public  TextMeshProUGUI dialogueCharacterNameText;
    public  GameObject      dialogueUI;
    private TextMeshProUGUI _dialogueLineText;
    private AnimateInText   _dialogueAnimateInText;
    #endregion

    #region Phone
    [Header("Phone")]
    public GameObject      phoneUi;
    public TextMeshProUGUI phoneSenderName;
    public TextMeshProUGUI phoneSenderText;
    public TextMeshProUGUI phoneChoice1Text;
    public TextMeshProUGUI phoneChoice2Text;
    public GameObject      phoneChoiceUI;
    public GameObject      phoneResponseUI;
    public TextMeshProUGUI responseText;
    public GameObject      phoneSingleBack;
    public GameObject      phoneNavigation;
    public TextMeshProUGUI phoneYouName;
    public TextMeshProUGUI phoneYouTime;
    #endregion
    
    #region General
    [Header("General")]
    public  Image         background; 
    public  Image         characterImage;
    public  NarrationItem startingNarrativeItem;
    private NarrationItem _currentNarrativeItem;
    public  ButtonManager buttonManager;
    #endregion

    #region Audio
    [Header("Audio")]
    public  AudioSource audioSource;
    private Coroutine   _audioCoroutine;
    #endregion
    
    #region Multi Choice Dialogue
    [Header("Multiple Choice Dialogue")]

    public GameObject      multiDialogueChoicePanel;
    public TextMeshProUGUI multiDialogueChoice1;
    public TextMeshProUGUI multiDialogueChoice2;
    #endregion
    

    private NarrativeHistory _narrativeHistory;

    private void Start() {
        _dialogueAnimateInText = dialogueArea.GetComponent<AnimateInText>();
        _dialogueLineText = dialogueArea.GetComponent<TextMeshProUGUI>();
        _narrativeHistory = GetComponent<NarrativeHistory>();
        _currentNarrativeItem = startingNarrativeItem;
        PrepareNarrativeArea();

        RunNarrativeItem();
    }

    public void AdvanceNarrative(int option = 0) {
        StopPreviousItem();
        dialogueUI.SetActive(true);
        phoneUi.SetActive(false);

        print("Advancing Narrative");
        
        // SaveChoice(option);
        // TODO  IMPORTANT when the current narrative item has next options that are dependent on previous choices, we need to enable and disable them based on previous choices
        _currentNarrativeItem = _currentNarrativeItem.next[option].narrativeItem;
        OpenPhone(option);

        PrepareNarrativeArea();
        RunNarrativeItem();
    }

    private void OpenPhone(int option) {
        if(!_currentNarrativeItem.phone) return;
        dialogueUI.SetActive(false);
        phoneUi.SetActive(true);
    }

    private IEnumerator PhoneOpeningSequence() {
        dialogueUI.SetActive(false);
        phoneUi.SetActive(true);
        yield break;
    }

    private void SaveChoice(int option) {
        _narrativeHistory.narrativeHistory[_currentNarrativeItem.character.name]=new CharacterHistory().AddHistory(_currentNarrativeItem.next[option].shortenedLine);
    }
    

    private void PrepareNarrativeArea() {
        if(_currentNarrativeItem.next.Count <1) return;
        characterImage.sprite = null;
        
    }

    public float sizeDeltaChange(int count) {
        return 20 * count * 2;
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

        if (!_currentNarrativeItem.character.name.Equals("Dmi")) {
            phoneSenderText.text = _currentNarrativeItem.line;
            phoneSenderName.text = _currentNarrativeItem.character.name;
            responseText.text = "";
            phoneYouName.text = "";
            phoneYouTime.text = "";
        }

        if (_currentNarrativeItem.character.name.Equals("Dmi")) {
            phoneChoiceUI.SetActive(false);
            phoneResponseUI.SetActive(true);
            responseText.text = _currentNarrativeItem.line;
            phoneYouName.text = $"{_currentNarrativeItem.character.name}: {_currentNarrativeItem.character.title}" ;
            phoneYouTime.text = "now";
        }

        if (_currentNarrativeItem.next.Count <2) {
            phoneNavigation.SetActive(true);
            phoneSingleBack.SetActive(false);
        }
        else {
            phoneNavigation.SetActive(false);
            phoneSingleBack.SetActive(true);
            phoneChoice1Text.text = _currentNarrativeItem.next[0].shortenedLine;
            phoneChoice2Text.text = _currentNarrativeItem.next[1].shortenedLine;

        }
        
        
        
    }

    private void UpdateSpokenText() {
        if(_currentNarrativeItem.phone) return;
        multiDialogueChoicePanel.SetActive(false);

        _dialogueLineText.fontStyle = FontStyles.Normal;
        if (_currentNarrativeItem.internalThought) {
            _dialogueLineText.fontStyle = FontStyles.Italic;
        }
        if (_currentNarrativeItem.physicalInteraction) {
            _dialogueLineText.fontStyle = FontStyles.Bold;
        }
        _dialogueLineText.text = _currentNarrativeItem.line;
        _dialogueAnimateInText.AnimateText();
        dialogueCharacterNameText.text = _currentNarrativeItem.character!=null? $"{_currentNarrativeItem.character.name}: {_currentNarrativeItem.character.title}":"";
        background.sprite = _currentNarrativeItem.background;
        if (_currentNarrativeItem.currentCharacterSprite.sprite != null) {
            characterImage.rectTransform.sizeDelta = _currentNarrativeItem.currentCharacterSprite.sprite.bounds.size*90;
            characterImage.sprite = _currentNarrativeItem.currentCharacterSprite.sprite;
        }

        if (_currentNarrativeItem.next.Count > 1) {
            multiDialogueChoicePanel.SetActive(true);
            multiDialogueChoice1.text = _currentNarrativeItem.next[0].shortenedLine;
            multiDialogueChoice2.text = _currentNarrativeItem.next[1].shortenedLine;

        }
    }

    private IEnumerator PlayAudioClips() {
        foreach (AudioClip clip in _currentNarrativeItem.sounds) {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }
    
}
