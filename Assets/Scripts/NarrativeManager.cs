using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : MonoBehaviour {
    public  GameObject       textArea;
    public  GameObject       characterNameText;
    public  GameObject       characterTitleText;
    public  Image            background; 
    public  NarrationItem    startingNarrativeItem;
    public  GameObject       multiInteractionButtonPrefab;
    public  GameObject       multiInteractionButtonParent;
    public  RectTransform    textBoxArea;
    private NarrationItem    _currentNarrativeItem;
    public  AudioSource      audioSource;
    private Coroutine        _audioCoroutine;
    private TMP_Text         _narrativeLineText;
    private AnimateInText    _animateInText;
    private TMP_Text         _characterName;
    private TMP_Text         _characterTitle;
    private List<GameObject> multiInteractionButtons = new();
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
        ClearNarrativeArea();
        SaveChoice(option);
        // TODO  IMPORTANT when the current narrative item has next options that are dependent on previous choices, we need to enable and disable them based on previous choices
        _currentNarrativeItem = _currentNarrativeItem.next[0].narrativeItem;
        PrepareNarrativeArea();
        RunNarrativeItem();
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
        _narrativeLineText.text = _currentNarrativeItem.line;
        _animateInText.AnimateText();
        _characterName.text = _currentNarrativeItem.character.name+":";
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
