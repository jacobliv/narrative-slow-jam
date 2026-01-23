using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetLanguageList : MonoBehaviour {
    public List<string> languageCodes;
    public GameObject languageList;
    public GameObject languageItemPrefab;
    public LocalizationManager localizationManager;
    private void OnEnable() {
        for (int i = 0; i < languageList.transform.childCount; i++) {
            Destroy(languageList.transform.GetChild(i).gameObject);
        }

        foreach (string languageCode in languageCodes) {
            GameObject languageObj = Instantiate(languageItemPrefab, languageList.transform, false);
            languageObj.SetActive(false);

            var text = languageObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.SetText(languageCode);

            var sel = languageObj.GetComponent<SelectLanguage>();
            sel.localizationManager = localizationManager;

            languageObj.SetActive(true);
        }
    }
}
