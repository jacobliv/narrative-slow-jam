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
    public  Button          nextButton;
    public  Button          backButton;
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
    public GameObject      alert;
    #endregion
    
    #region General
    [Header("General")]
    public  Image         background; 
    public Image            characterImage;
    public GameObject       characterCanvas;
    public GameObject       mainBackgroundCanvas;
    public List<GameObject> otherCanvases;
    public NarrationItem    startingNarrativeItem;
    public NarrationItem    currentNarrativeItem;
    public ButtonManager    buttonManager;
    public GameObject       frontShopInteractionParent;
    public GameObject       flubberCanvas;
    public GameObject       creditsCanvas;
    #endregion

    #region Audio
    [Header("Audio")]
    public  AudioSource audioSource;
    private Coroutine   _audioCoroutine;
    #endregion
    
    #region Multi Choice Dialogue
    [Header("Multiple Choice Dialogue")]
    public GameObject dialogueNavigationButtonPanel;
    
    public GameObject      multiDialogueChoicePanel;
    public TextMeshProUGUI multiDialogueChoice1;
    public TextMeshProUGUI multiDialogueChoice2;
    #endregion

    #region Characters

    [Header("Characters")] public List<Character> characters;
    public                        bool            flubberGone;
    #endregion

    [Header("Supernova")] public GameObject             twoCharacterCanvas;
    public                       GameObject             supernovaCanvas;
    public                       ControlBackgroundMusic ControlBackgroundMusic;
    public                       FadeIn                 fadeIn;


    private NarrativeHistory _narrativeHistory;
    private bool             isFlubberDisabled;

    private void Start() {
        _dialogueAnimateInText = dialogueArea.GetComponent<AnimateInText>();
        _dialogueLineText = dialogueArea.GetComponent<TextMeshProUGUI>();
        _narrativeHistory = GetComponent<NarrativeHistory>();
        _narrativeHistory.Reset();
        currentNarrativeItem = startingNarrativeItem;
        PrepareNarrativeArea();

        RunNarrativeItem();
    }

    public void AdvanceNarrative(int option = 0) {
        StopPreviousItem();
        if (currentNarrativeItem.next.Count == 0) {
            creditsCanvas.SetActive(true);
            return;
        }
        dialogueUI.SetActive(true);
        if (currentNarrativeItem.name.Contains("D2D-52")) {
            flubberGone = true;
        }

        if (option != -1) {
            if (currentNarrativeItem.name.Equals("[D3D-16b]")) {
                option = _narrativeHistory.positiveActions >= .75f * _narrativeHistory.choices ? 1 : 0;
            }
            // TODO  IMPORTANT when the current narrative item has next options that are dependent on previous choices, we need to enable and disable them based on previous choices
            if (currentNarrativeItem.next.Count - 1 < option || currentNarrativeItem.next[option].narrativeItem == null) {
                Debug.LogWarning($"Current narrative doesn't have a next at index {option}");
                return;
            }
            SaveChoice(option);

            currentNarrativeItem = currentNarrativeItem.next[option].narrativeItem;
        }
        
        OpenPhone();

        PrepareNarrativeArea();
        RunNarrativeItem();
    }

    private void OpenPhone() {
        if (!currentNarrativeItem.phone) {
            phoneUi.SetActive(false);

            return;
        }
        dialogueUI.SetActive(false);
        if (currentNarrativeItem.phone && !phoneUi.activeSelf) {
            phoneUi.SetActive(true);

        } 
    }



    private void SaveChoice(int option) {
        _narrativeHistory.linearHistory.Add(currentNarrativeItem);

        _narrativeHistory.AddNarrativeHistory(currentNarrativeItem,option);
    }

    public void GoBack() {
        if( _narrativeHistory.positiveValue.ContainsKey(currentNarrativeItem.name)) {
            _narrativeHistory.positiveActions -= _narrativeHistory.positiveValue[currentNarrativeItem.name];
            _narrativeHistory.choices--;
        }
        if (currentNarrativeItem.name.Equals("[POP-19]")) {
            twoCharacterCanvas.SetActive(false);
        }
        currentNarrativeItem = _narrativeHistory.linearHistory[^1];
        _narrativeHistory.linearHistory.RemoveAt(_narrativeHistory.linearHistory.Count-1);
        AdvanceNarrative(-1);
    }
    

    private void PrepareNarrativeArea() {
        if(currentNarrativeItem.next.Count <1) return;
        characterImage.sprite = null;
        characterImage.color=Color.clear;
        if (!currentNarrativeItem.phone) {
            otherCanvases.ForEach((c)=>c.SetActive(false));
            mainBackgroundCanvas.SetActive(true);
        }
        
        
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
        if(currentNarrativeItem.name.Equals("O1")) {
            backButton.gameObject.SetActive(false);
        } else if (!backButton.gameObject.activeSelf) {
            backButton.gameObject.SetActive(true);
        }
        if (currentNarrativeItem.name.Equals("[PON-12]") || currentNarrativeItem.name.Equals("[POP-71]")) {
            supernovaCanvas.SetActive(true);
            fadeIn.FadeInFunc();
        }
        
        if (currentNarrativeItem.name.Equals("[POP-18]") || currentNarrativeItem.name.Equals("[PON-1]")) {
            PersistentObject.instance.GetComponent<ControlBackgroundMusic>().ChangeToEndSong();
        }
        if (currentNarrativeItem.name.Equals("[POP-19]")) {
            twoCharacterCanvas.SetActive(true);
        }
        if (currentNarrativeItem.name.Equals("[TPOP-1]")) {
            alert.SetActive(true);
            phoneSingleBack.SetActive(false);
            phoneNavigation.SetActive(true);
            return;
        }
        // update text area
        UpdateSpokenText();
        UpdatePhoneText();


        // update background
        background.sprite = currentNarrativeItem.background;
        if (currentNarrativeItem.shopSelection) {
            frontShopInteractionParent.SetActive(true);
        }
        else {
            frontShopInteractionParent.SetActive(false);
        }

        // update characters
        _audioCoroutine=StartCoroutine(PlayAudioClips());
        if (background.sprite.name.Contains("Shop") && currentNarrativeItem.day is not (Day.Three or Day.Post) && !flubberGone) {
            flubberCanvas.SetActive(true);
        }
        else {
            flubberCanvas.SetActive(false);

        }

        if (currentNarrativeItem.day == Day.Post) {
            creditsCanvas.SetActive(true);
        }
        else {
            creditsCanvas.SetActive(false);
        }
        
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
            phoneYouName.text = $"{currentNarrativeItem.character.name}" ;
            phoneYouTime.text = "now";
        }

        if (currentNarrativeItem.next.Count <2) {
            phoneNavigation.SetActive(true);
            phoneSingleBack.SetActive(false);
        }
        else {
            phoneNavigation.SetActive(false);
            phoneSingleBack.SetActive(true);
            phoneChoiceUI.SetActive(true);
            phoneResponseUI.SetActive(false);
            phoneChoice1Text.text = currentNarrativeItem.next[0].shortenedLine;
            phoneChoice2Text.text = currentNarrativeItem.next[1].shortenedLine;

        }
        
        
        
    }

    private void UpdateSpokenText() {
        if(currentNarrativeItem.phone) return;
        multiDialogueChoicePanel.SetActive(false);
        dialogueNavigationButtonPanel.SetActive(true);
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
        if (currentNarrativeItem.characterArt != CharacterEnum.None) {
            var sprite = characters.Find((c)=>c.name.Equals("Rob")).sprite;
            switch (currentNarrativeItem.characterArt) {
                case CharacterEnum.Rob:
                    sprite = characters.Find((c)=>c.name.Equals("Rob")).sprite;
                    break;
                case CharacterEnum.Dmi:
                    sprite = characters.Find((c)=>c.name.Equals("Dmi")).sprite;
                    break;
                case CharacterEnum.Cassiopeia:
                    sprite = characters.Find((c)=>c.name.Equals("Cassiopeia")).sprite;
                    break;
                case CharacterEnum.BOO8:
                    sprite = characters.Find((c)=>c.name.Equals("8008")).sprite;
                    break;
                case CharacterEnum.Aiyana:
                    sprite = characters.Find((c)=>c.name.Equals("Aiyana")).sprite;
                    break;
                case CharacterEnum.Calder:
                    sprite = characters.Find((c)=>c.name.Equals("Calder")).sprite;
                    break;
                case CharacterEnum.Flubber:
                    sprite = characters.Find((c)=>c.name.Equals("Flubber")).sprite;
                    break;
                case CharacterEnum.Kkili:
                    sprite = characters.Find((c)=>c.name.Equals("K'Kili the Destroyer")).sprite;
                    break;
                case CharacterEnum.RemRom:
                    sprite = characters.Find((c)=>c.name.Equals("Remus")).sprite;
                    break;
                case CharacterEnum.Zerua:
                    sprite = characters.Find((c)=>c.name.Equals("Zerua")).sprite;
                    break;
                case CharacterEnum.Crust:
                    sprite = characters.Find((c)=>c.name.Equals("Crust")).sprite;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            characterCanvas.SetActive(true);
            characterImage.rectTransform.sizeDelta = sprite.bounds.size*20;
            characterImage.sprite = sprite;
            characterImage.color=Color.white;
        }
        else{
            characterCanvas.SetActive(false);
        }
        if (currentNarrativeItem.next.Count > 1 && currentNarrativeItem.next[0].button.Equals("Dialogue Choice 1")) {
            multiDialogueChoicePanel.SetActive(true);
            dialogueNavigationButtonPanel.SetActive(false);
            multiDialogueChoice1.text = currentNarrativeItem.next[0].shortenedLine;
            multiDialogueChoice2.text = currentNarrativeItem.next[1].shortenedLine;
        } 
        else if (currentNarrativeItem.next.Count > 1 && currentNarrativeItem.shopSelection) {
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
        if (currentNarrativeItem.next[0].button.Equals(buttonName) || currentNarrativeItem.next[0].button.Equals("")) {
            AdvanceNarrative(0);
            return;
        }
        AdvanceNarrative(1);
    }

    public bool FlubberCanvasDisabled() {
        return flubberGone;
    }
    
}
