using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLanguage : MonoBehaviour {
    public LocalizationManager localizationManager;
    // TextMeshPro Dropdown
    public TMPro.TMP_Dropdown dropdown;
    public void ChangeLanguage(int index) {
        Debug.Log("Changing language to: " + index);
        Debug.Log("Language: " + dropdown.options[index].text);
        string code = dropdown.options[index].text;
        Language language = localizationManager.languages.Find(l => l.Code == code);
        localizationManager.LoadLocalization(language);
    }
}
