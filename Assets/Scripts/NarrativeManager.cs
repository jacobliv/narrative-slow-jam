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
    public  Image         characterImage;

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
    private List<GameObject> multiInteractionButtons = new();

    private TMP_Text      _characterTitle;
    public  GameObject       multiInteractionButtonPrefab;
    public  GameObject       multiInteractionButtonParent;
    public  RectTransform    textBoxArea;
    private NarrativeHistory _narrativeHistory;

    private void Start() {
        _characterTitle = characterTitleText.GetComponent<TMP_Text>();
        _characterName = characterNameText.GetComponent<TMP_Text>();
        _animateInText = textArea.GetComponent<AnimateInText>();
        _narrativeLineText = textArea.GetComponent<TMP_Text>();
        _narrativeHistory = GetComponent<NarrativeHistory>();
        _currentNarrativeItem = startingNarrativeItem;
        PrepareNarrativeArea();

        RunNarrativeItem();
    }

    private void Update() {

        if (Input.GetMouseButtonUp(0) && /*_currentNarrativeItem.next.Count == 1 &&*/ _currentNarrativeItem.next[0].button == null) {
            AdvanceNarrative(0);
        }
    }

    public void AdvanceNarrative(int option = 0) {
        StopPreviousItem();
        spokenTextUi.SetActive(true);

        OpenPhone(option);
        
        _currentNarrativeItem = _currentNarrativeItem.next[option].narrativeItem;
        RunNarrativeItem();
        ClearNarrativeArea();
        SaveChoice(option);
        // TODO  IMPORTANT when the current narrative item has next options that are dependent on previous choices, we need to enable and disable them based on previous choices
        _currentNarrativeItem = _currentNarrativeItem.next[0].narrativeItem;
        PrepareNarrativeArea();
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

    private void SaveChoice(int option) {
        _narrativeHistory.narrativeHistory[_currentNarrativeItem.character.name]=new CharacterHistory().AddHistory(_currentNarrativeItem.next[option].shortenedLine);
    }

    private void ClearNarrativeArea() {
        textBoxArea.sizeDelta = new Vector2(textBoxArea.sizeDelta.x,
                                            textBoxArea.sizeDelta.y - sizeDeltaChange(multiInteractionButtons.Count) );
        foreach (GameObject multiInteractionButton in multiInteractionButtons) {
            Destroy(multiInteractionButton);
        }
        multiInteractionButtons.Clear();
    }

    private void PrepareNarrativeArea() {
        if(_currentNarrativeItem.next.Count <=1) return;
        int i = 0;
        foreach (NextNarrative nextNarrative in _currentNarrativeItem.next) {
            GameObject button = Instantiate(multiInteractionButtonPrefab, multiInteractionButtonParent.transform);
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = nextNarrative.shortenedLine;
            button.GetComponent<Button>().onClick.AddListener(()=>AdvanceNarrative(i));
            multiInteractionButtons.Add(button);
            i++;
        }

        textBoxArea.sizeDelta = new Vector2(textBoxArea.sizeDelta.x,
                                            textBoxArea.sizeDelta.y + sizeDeltaChange(_currentNarrativeItem.next.Count) );
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
        phoneText.text = _currentNarrativeItem.line;
        phoneName.text = _currentNarrativeItem.character.name;

    }

    private void UpdateSpokenText() {
        if(_currentNarrativeItem.phone) return;
        _narrativeLineText.text = _currentNarrativeItem.line;
        _animateInText.AnimateText();
        _characterName.text = _currentNarrativeItem.character.name+":";
        _characterTitle.text = _currentNarrativeItem.character.title;
        background.sprite = _currentNarrativeItem.background;
        if (_currentNarrativeItem.currentCharacterSprite.sprite != null) {
            characterImage.rectTransform.sizeDelta = _currentNarrativeItem.currentCharacterSprite.sprite.bounds.size*90;
            characterImage.sprite = _currentNarrativeItem.currentCharacterSprite.sprite;
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
