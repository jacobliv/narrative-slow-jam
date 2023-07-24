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
    public Button nextButton;
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
    public NarrationItem currentNarrativeItem;
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
        currentNarrativeItem = startingNarrativeItem;
        PrepareNarrativeArea();

        RunNarrativeItem();
    }

    public void AdvanceNarrative(int option = 0) {
        StopPreviousItem();
        dialogueUI.SetActive(true);
        phoneUi.SetActive(false);

        print("Advancing Narrative");
        if (option != -1) {
            SaveChoice(option);
            // TODO  IMPORTANT when the current narrative item has next options that are dependent on previous choices, we need to enable and disable them based on previous choices
            if (currentNarrativeItem.next.Count - 1 < option || currentNarrativeItem.next[option].narrativeItem == null) {
                Debug.LogWarning($"Current narrative doesn't have a next at index {option}");
                return;
            }
            currentNarrativeItem = currentNarrativeItem.next[option].narrativeItem;
            OpenPhone(option);
        }
        

        PrepareNarrativeArea();
        RunNarrativeItem();
    }

    private void OpenPhone(int option) {
        if(!currentNarrativeItem.phone) return;
        dialogueUI.SetActive(false);
        phoneUi.SetActive(true);
    }

    private IEnumerator PhoneOpeningSequence() {
        dialogueUI.SetActive(false);
        phoneUi.SetActive(true);
        yield break;
    }

    private void SaveChoice(int option) {
        _narrativeHistory.AddNarrativeHistory(currentNarrativeItem,option);
        _narrativeHistory.linearHistory.Add(currentNarrativeItem);
    }

    public void GoBack() {
        currentNarrativeItem = _narrativeHistory.linearHistory[^1];
        _narrativeHistory.linearHistory.RemoveAt(_narrativeHistory.linearHistory.Count-1);
        AdvanceNarrative(-1);
    }
    

    private void PrepareNarrativeArea() {
        if(currentNarrativeItem.next.Count <1) return;
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
        if (currentNarrativeItem == null) return;
        // update text area
        UpdateSpokenText();
        UpdatePhoneText();


        // update background
        background.sprite = currentNarrativeItem.background;

        // update characters
        _audioCoroutine=StartCoroutine(PlayAudioClips());

        
    }

    private void UpdatePhoneText() {
        if(!currentNarrativeItem.phone) return;

        if (!currentNarrativeItem.character.name.Equals("Dmi")) {
            phoneSenderText.text = currentNarrativeItem.line;
            phoneSenderName.text = currentNarrativeItem.character.name;
            responseText.text = "";
            phoneYouName.text = "";
            phoneYouTime.text = "";
        }

        if (currentNarrativeItem.character.name.Equals("Dmi")) {
            phoneChoiceUI.SetActive(false);
            phoneResponseUI.SetActive(true);
            responseText.text = currentNarrativeItem.line;
            phoneYouName.text = $"{currentNarrativeItem.character.name}: {currentNarrativeItem.character.title}" ;
            phoneYouTime.text = "now";
        }

        if (currentNarrativeItem.next.Count <2) {
            phoneNavigation.SetActive(true);
            phoneSingleBack.SetActive(false);
        }
        else {
            phoneNavigation.SetActive(false);
            phoneSingleBack.SetActive(true);
            phoneChoice1Text.text = currentNarrativeItem.next[0].shortenedLine;
            phoneChoice2Text.text = currentNarrativeItem.next[1].shortenedLine;

        }
        
        
        
    }

    private void UpdateSpokenText() {
        if(currentNarrativeItem.phone) return;
        multiDialogueChoicePanel.SetActive(false);
        nextButton.transform.gameObject.SetActive(true);
        _dialogueLineText.fontStyle = FontStyles.Normal;
        if (currentNarrativeItem.internalThought) {
            _dialogueLineText.fontStyle = FontStyles.Italic;
        }
        if (currentNarrativeItem.physicalInteraction) {
            _dialogueLineText.fontStyle = FontStyles.Bold;
        }
        _dialogueLineText.text = currentNarrativeItem.line;
        _dialogueAnimateInText.AnimateText();
        dialogueCharacterNameText.text = currentNarrativeItem.character!=null? $"{currentNarrativeItem.character.name}: {currentNarrativeItem.character.title}":"";
        background.sprite = currentNarrativeItem.background;
        if (currentNarrativeItem.currentCharacterSprite.sprite != null) {
            characterImage.rectTransform.sizeDelta = currentNarrativeItem.currentCharacterSprite.sprite.bounds.size*90;
            characterImage.sprite = currentNarrativeItem.currentCharacterSprite.sprite;
        }

        if (currentNarrativeItem.next.Count > 1 && currentNarrativeItem.next[0].button.Equals("Dialogue Choice 1")) {
            multiDialogueChoicePanel.SetActive(true);
            multiDialogueChoice1.text = currentNarrativeItem.next[0].shortenedLine;
            multiDialogueChoice2.text = currentNarrativeItem.next[1].shortenedLine;
        } 
        else if (currentNarrativeItem.next.Count > 1) {
            nextButton.transform.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayAudioClips() {
        foreach (AudioClip clip in currentNarrativeItem.sounds) {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }

    public void ShopButtonAdvance(string buttonName) {
        if (currentNarrativeItem.next[0].button.Equals(buttonName)) {
            AdvanceNarrative(0);
            return;
        }
        AdvanceNarrative(1);
    }
    
}
