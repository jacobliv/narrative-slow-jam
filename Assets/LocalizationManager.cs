using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
        CurrentLanguage = ResolveInitialLanguage();
        StaticLanguage = CurrentLanguage;

        if (CurrentLanguage != null) {
            LoadLocalization(CurrentLanguage);
        }
        else {
            Debug.LogError("LocalizationManager could not resolve an initial language.");
        }
    }

    private Language ResolveInitialLanguage() {
        if (StaticLanguage != null) {
            return StaticLanguage;
        }

        if (CurrentLanguage != null) {
            return CurrentLanguage;
        }

        if (languageDatabase == null || languageDatabase.languages == null || languageDatabase.languages.Count == 0) {
            Debug.LogWarning("LocalizationManager has no languages available in the database.");
            return null;
        }

        int safeIndex = Mathf.Clamp(currentLanguageIndex, 0, languageDatabase.languages.Count - 1);
        var fallbackLanguage = languageDatabase.languages[safeIndex];
        return fallbackLanguage;
    }


    public void GetCurrentLocalization() {
        string localization = GetLocalization(currentType, currentKey);
        Debug.Log("Localization: " + localization);
    }

    public void GetCurrentLanguage() {
        LoadLocalization(CurrentLanguage);
    } 
    
    public string GetLocalization(LocalizationType type, string key) {
        return GetLocalization(type, key, key);
    }

    public string GetLocalization(LocalizationType type, string key, string englishFallback) {
        var table = GetLocalizationTable(type);
        if (table != null && !string.IsNullOrEmpty(key) && table.TryGetValue(key, out string localizedValue) && !string.IsNullOrEmpty(localizedValue)) {
            return localizedValue;
        }

        return englishFallback ?? "";
    }
    
    public TMP_FontAsset GetFont(bool bold) {
        FontLanguageMapping mapping = fontMappingDatabase.mappings.Find(m => m.languageCode == CurrentLanguage.Code);
        if (mapping == null) mapping = fontMappingDatabase.mappings.Find(m => m.languageCode == "en");
        if (bold) return mapping.bold;
        return mapping.normal;
    }


    private string DecodeLocalizationText(TextAsset ta, Language language) {
        if (ta == null) {
            return string.Empty;
        }

        byte[] bytes = ta.bytes;
        if (bytes == null || bytes.Length == 0) {
            return ta.text ?? string.Empty;
        }

        try {
            return SanitizeDecodedLocalizationText(new UTF8Encoding(false, true).GetString(bytes));
        }
        catch (DecoderFallbackException) {
            if (language != null && language.Code == "th") {
                return SanitizeDecodedLocalizationText(Encoding.GetEncoding(874).GetString(bytes));
            }

            return SanitizeDecodedLocalizationText(ta.text ?? string.Empty);
        }
    }

    private string SanitizeDecodedLocalizationText(string text) {
        if (string.IsNullOrEmpty(text)) {
            return string.Empty;
        }

        return text.TrimStart('\uFEFF');
    }

    // CSV loader utility
    private Dictionary<string, string> ParseCsvWithHelper(TextAsset ta, string keyColumn, string valueColumn, Language language) {
        var dict = new Dictionary<string, string>();
        using (var reader = new StringReader(DecodeLocalizationText(ta, language)))
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
        if (language == null) {
            Debug.LogError("LoadLocalization called with null language.");
            return;
        }

        StaticLanguage = language;
        CurrentLanguage = language;
        UpdateCurrentLanguageIndex(language);

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
            characterNames = ParseCsvWithHelper(charFile, "character name", "translated name", language);
            characterTitles = ParseCsvWithHelper(charFile, "character name", "translated title", language);
        }

        var menuFile = FindLocalizationFile(language, menuSuffix);
        Debug.Log("Menu File: " + menuFile);
        if (menuFile != null) menusAndExtras = ParseCsvWithHelper(menuFile, "location", "target", language);

        var scriptFile = FindLocalizationFile(language, scriptSuffix);
        if (scriptFile != null) scriptLocalization = ParseCsvWithHelper(scriptFile, "location", "target", language);
    }

    private void UpdateCurrentLanguageIndex(Language language) {
        if (languageDatabase == null || languageDatabase.languages == null) {
            return;
        }

        int index = languageDatabase.languages.FindIndex(l => l.Code == language.Code);
        if (index >= 0) {
            currentLanguageIndex = index;
        }
    }

    private Dictionary<string, string> GetLocalizationTable(LocalizationType type) {
        switch (type) {
            case LocalizationType.Script:
                return scriptLocalization;
            case LocalizationType.Menu:
                return menusAndExtras;
            case LocalizationType.CharacterName:
                return characterNames;
            case LocalizationType.CharacterTitle:
                return characterTitles;
            default:
                return null;
        }
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
    public string DisplayName;
    public string Code;

    public override string ToString() {
        return $"{nameof(Name)}: {Name}, {nameof(Code)}: {Code}";
    }
}
