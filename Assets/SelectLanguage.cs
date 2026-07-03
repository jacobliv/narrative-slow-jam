using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLanguage : MonoBehaviour
{
    public LocalizationManager localizationManager;
    public TextMeshProUGUI languageName;
    private Coroutine initializeCoroutine;

    private void OnEnable()
    {
        StartInitialization();
    }

    private void OnDisable()
    {
        if (initializeCoroutine != null)
        {
            StopCoroutine(initializeCoroutine);
            initializeCoroutine = null;
        }
    }

    public void Refresh()
    {
        StartInitialization();
    }

    private void StartInitialization()
    {
        if (initializeCoroutine != null)
        {
            StopCoroutine(initializeCoroutine);
        }

        initializeCoroutine = StartCoroutine(InitializeWhenReady());
    }

    private IEnumerator InitializeWhenReady()
    {
        while (!TryApplyState())
        {
            yield return null;
        }

        initializeCoroutine = null;
    }

    private bool TryApplyState()
    {
        if (localizationManager == null)
        {
            localizationManager = FindObjectOfType<LocalizationManager>();
            if (localizationManager == null) return false;
        }

        if (localizationManager.CurrentLanguage == null)
        {
            return false;
        }

        var codeLabel = transform.childCount > 0 ? transform.GetChild(0).GetComponent<TextMeshProUGUI>() : null;
        if (codeLabel != null)
        {
            string code = codeLabel.text;
            if (localizationManager.CurrentLanguage.Code.Equals(code))
            {
                var button = GetComponent<Button>();
                if (button != null)
                {
                    button.Select();
                }
            }
        }

        if (languageName != null)
        {
            languageName.font = localizationManager.GetFont(false);
            languageName.SetText(localizationManager.CurrentLanguage.DisplayName);
        }

        return true;
    }

    public void ChangeLanguageByStep(int direction)
    {
        if (localizationManager == null)
        {
            localizationManager = FindObjectOfType<LocalizationManager>();
        }

        if (localizationManager == null || localizationManager.languageDatabase == null || localizationManager.languageDatabase.languages == null || localizationManager.languageDatabase.languages.Count == 0)
        {
            return;
        }

        int step = direction < 0 ? -1 : 1;
        int languageCount = localizationManager.languageDatabase.languages.Count;
        int nextIndex = (localizationManager.currentLanguageIndex + step + languageCount) % languageCount;
        Language nextLanguage = localizationManager.languageDatabase.languages[nextIndex];

        localizationManager.LoadLocalization(nextLanguage);
        TryApplyState();

    }

    public void ChangeLanguage()
    {
        if (localizationManager == null || localizationManager.languageDatabase == null)
        {
            Debug.LogWarning("Localization manager is not ready yet.");
            return;
        }

        string code = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        Language language = localizationManager.languageDatabase.languages.Find(l => l.Code == code);
        if (language == null)
        {
            Debug.LogWarning("Could not find language for code: " + code);
            return;
        }

        localizationManager.LoadLocalization(language);
        TryApplyState();
    }
}
