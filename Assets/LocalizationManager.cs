using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {
    public        Dictionary<string,string>  scriptLocalization;
    public        Dictionary<string, string> menusAndExtras;
    public        Dictionary<string, string> characterNames;
    public        Dictionary<string, string> characterTitles;
    public        string                     currentKey; // For debugging
    public        LocalizationType           currentType; // For debugging  
    public        FontMappingDatabase        fontMappingDatabase;
    public        LanguageDatabase           languageDatabase;
    public        int                        currentLanguageIndex = 0;
    public        Language                   CurrentLanguage;
    public static Language                   StaticLanguage;

    public void Awake() {
        if (StaticLanguage == null) StaticLanguage = CurrentLanguage;
        CurrentLanguage = StaticLanguage;
        LoadLocalization(CurrentLanguage);
    }


    public void GetCurrentLocalization() {
        string localization = GetLocalization(currentType, currentKey);
        Debug.Log("Localization: " + localization);
    }

    public void GetCurrentLanguage() {
        LoadLocalization(CurrentLanguage);
    } 
    
    public string GetLocalization(LocalizationType type, string key) {
        switch (type) {
            case LocalizationType.Script:
                return scriptLocalization[key];
            case LocalizationType.Menu:
                return menusAndExtras[key];
            case LocalizationType.CharacterName:
                return characterNames[key];
            case LocalizationType.CharacterTitle:
                return characterTitles[key];
            default:
                return "";
        }
    }
    
    public TMP_FontAsset GetFont(bool bold) {
        FontLanguageMapping mapping = fontMappingDatabase.mappings.Find(m => m.languageCode == CurrentLanguage.Code);
        if (mapping == null) mapping = fontMappingDatabase.mappings.Find(m => m.languageCode == "en");
        if (bold) return mapping.bold;
        return mapping.normal;
    }


    // CSV loader utility
    private Dictionary<string, string> ParseCsvWithHelper(TextAsset ta, string keyColumn, string valueColumn) {
        var dict = new Dictionary<string, string>();
        using (var reader = new StringReader(ta.text))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
            // Reads header: location,source,target
            csv.Read();
            csv.ReadHeader();

            while (csv.Read()) {
                string location = csv.GetField(keyColumn);
                string target = csv.GetField(valueColumn);
                if (!string.IsNullOrEmpty(location))
                    dict[location] = target;
            }
        }

        Debug.Log("Successfully parsed {" + ta.name + "}");
        return dict;
    }


    // Try to find a localization CSV with a wildcard date
    private TextAsset FindLocalizationFile(Language language, string typeSuffix) {
        // Load all TextAssets in the language folder
        var allTextAssets = Resources.LoadAll<TextAsset>("language");
        foreach (var ta in allTextAssets) {
            var name = ta.name;
            // Check for pattern: {lang}_{any}_typeSuffix.csv
            if (name.StartsWith(language.Code + "_") && name.EndsWith(typeSuffix)) {
                return ta;
            }
        }
        return null;
    }

    public void LoadLocalization(Language language) {
        StaticLanguage = language;
        CurrentLanguage = language;

        characterNames = new Dictionary<string, string>();
        menusAndExtras = new Dictionary<string, string>();
        scriptLocalization = new Dictionary<string, string>();

        // The type-specific file suffixes (without file extension)
        string charSuffix = "Character_Titles+Names";
        string menuSuffix = "MenuAndExtras";
        string scriptSuffix = "Script";

        // Find and load each localization file
        var charFile = FindLocalizationFile(language, charSuffix);
        if (charFile != null) {
            characterNames = ParseCsvWithHelper(charFile, "character name", "translated name");
            characterTitles = ParseCsvWithHelper(charFile, "character name", "translated title");
        }

        var menuFile = FindLocalizationFile(language, menuSuffix);
        Debug.Log("Menu File: " + menuFile);
        if (menuFile != null) menusAndExtras = ParseCsvWithHelper(menuFile, "location", "target");

        var scriptFile = FindLocalizationFile(language, scriptSuffix);
        if (scriptFile != null) scriptLocalization = ParseCsvWithHelper(scriptFile, "location", "target");
    }

}

public enum LocalizationType {
    Script,
    Menu,
    CharacterName,
    CharacterTitle,
}

[Serializable]
public class FontLanguageMapping {
	[SerializeField]
	public string languageCode;	
    [SerializeField]
	public TMP_FontAsset normal;
    [SerializeField]
    public TMP_FontAsset bold;
    [SerializeField]
    public TMP_FontAsset medium;
    
}

[System.Serializable]
public class Language {
    public Language(string name, string code) {
        Name = name;
        Code = code;
    }
    public string Name;
    public string Code;

    public override string ToString() {
        return $"{nameof(Name)}: {Name}, {nameof(Code)}: {Code}";
    }
}
