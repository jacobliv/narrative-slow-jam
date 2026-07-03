using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SetLanguageList : MonoBehaviour {
    public List<string> languageCodes;
    public GameObject languageList;
    public GameObject languageItemPrefab;
    public LocalizationManager localizationManager;
    private void OnEnable() {
        if (localizationManager == null) {
            localizationManager = FindObjectOfType<LocalizationManager>();
        }

        for (int i = languageList.transform.childCount - 1; i >= 0; i--) {
            Destroy(languageList.transform.GetChild(i).gameObject);
        }

        IEnumerable<string> codes = GetLanguageCodes();
        foreach (string languageCode in codes) {
            GameObject languageObj = Instantiate(languageItemPrefab, languageList.transform, false);
            languageObj.SetActive(false);

            var text = languageObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.SetText(languageCode);

            var sel = languageObj.GetComponent<SelectLanguage>();
            sel.localizationManager = localizationManager;

            languageObj.SetActive(true);
            sel.Refresh();
        }
    }

    private IEnumerable<string> GetLanguageCodes() {
        if (localizationManager != null &&
            localizationManager.languageDatabase != null &&
            localizationManager.languageDatabase.languages != null &&
            localizationManager.languageDatabase.languages.Count > 0) {
            return localizationManager.languageDatabase.languages
                .Where(language => language != null && !string.IsNullOrEmpty(language.Code))
                .Select(language => language.Code);
        }

        return languageCodes ?? new List<string>();
    }
}
