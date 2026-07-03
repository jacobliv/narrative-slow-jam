using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationRetriever : MonoBehaviour {
    public LocalizationManager localizationManager;

    public string GetLocalization(LocalizationType type, string key, string current) {
        key = key.Replace("[", "").Replace("]", "");
        if (localizationManager.CurrentLanguage.Code.Equals("en")) return current;
        return localizationManager.GetLocalization(type, key, current);
    }

    public TMP_FontAsset GetFont(bool bold) {
        return localizationManager.GetFont(bold);
    }
}
