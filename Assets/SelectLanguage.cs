using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLanguage : MonoBehaviour {
    public LocalizationManager localizationManager;

    private void OnEnable() {
        string code = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        Debug.Log("Language: " + code);
        Debug.Log("Current Language: " + localizationManager.CurrentLanguage);
        if (localizationManager.CurrentLanguage.Code.Equals(code)) {
            gameObject.GetComponent<Button>().Select();
        }
    }

    public void ChangeLanguage() {
        string code = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        Debug.Log("Language: " + code);
        Language language = localizationManager.languageDatabase.languages.Find(l => l.Code == code);
        localizationManager.LoadLocalization(language);
    }
}
