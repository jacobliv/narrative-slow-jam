using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticTextLocalization : MonoBehaviour {
    public LocalizationRetriever retriever;
    public TextMeshProUGUI       text;
    public LocalizationType      type;
    public string                key;
    public string                englishLine;

    public void Start() {
        Debug.Log("Getting Localization for: " +gameObject.name +" -- " + transform.parent.name);
        text.text = retriever.GetLocalization(type, key, englishLine);
    }
    
    public void Update() {
        text.text = retriever.GetLocalization(type, key, englishLine);

    }
}
