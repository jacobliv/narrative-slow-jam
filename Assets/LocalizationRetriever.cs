using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationRetriever : MonoBehaviour {
    public LocalizationManager localizationManager;

    public string GetLocalization(LocalizationType type, string key, string current) {
        key = key.Replace("[", "").Replace("]", "");
        Debug.Log("Getting localization for " + key);
        Debug.Log("Current language: " + localizationManager.CurrentLanguage);
        if (localizationManager.CurrentLanguage.Code.Equals("en")) return current;
        return localizationManager.GetLocalization(type, key);
    }
}
