using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveOrLoad : MonoBehaviour,IPointerClickHandler {
    public                   Image            targetImage; 
    private                  Texture2D        _screenshotTexture;
    [SerializeField] private NarrativeManager narrativeManager;
    [SerializeField] private NarrativeHistory narrativeHistory;


    public void OnPointerClick(PointerEventData eventData) {
        Save();
    }

    private void Save() {
        SaveData saveData = new SaveData {
            historyChoices = narrativeHistory,
            currentNarrative = narrativeManager.CurrentNarrativeItem,
            currentDay = (int)narrativeManager.CurrentNarrativeItem.day
        };
        Debug.Log(narrativeManager.CurrentNarrativeItem.name);
        StartCoroutine(TakeScreenshot());
        // AssetDatabase.getAss(narrativeManager.CurrentNarrativeItem.GetInstanceID())[0].v
        string json = JsonHelper.ToJson<HistoryItem>(narrativeHistory.narrativeHistory.ToArray());
        Debug.Log(json);
        // PlayerPrefs.SetString("SaveData", json);
        // PlayerPrefs.Save();

        
    }
    
    IEnumerator TakeScreenshot() {
        yield return new WaitForEndOfFrame();
        _screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _screenshotTexture.Apply();
        if (targetImage != null) {
            targetImage.sprite = Sprite.Create(_screenshotTexture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
        }
    }
}

[Serializable]
public class SaveData {
    [SerializeField]
    public NarrativeHistory historyChoices;
    [SerializeField]

    public NarrationItem                               currentNarrative;
    [SerializeField]

    public int                                  currentDay;
    // Add other variables as needed
}