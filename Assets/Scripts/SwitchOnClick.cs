using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SwitchOnClick : MonoBehaviour {
    public UnityEvent onClick;

    void Update() {
        if (!Input.anyKeyDown) {
            return;
        }

        if (TryAdvanceCreditsPage()) {
            onClick.Invoke();
            return;
        }

        SceneManager.LoadScene("StartMenu");
        onClick.Invoke();
    }

    private bool TryAdvanceCreditsPage() {
        string nextCreditsCanvasName = GetNextCreditsCanvasName();
        if (string.IsNullOrEmpty(nextCreditsCanvasName)) {
            return false;
        }

        GameObject nextCreditsCanvas = FindInactiveSceneObject(nextCreditsCanvasName);
        if (nextCreditsCanvas == null) {
            Debug.LogWarning($"Could not find next credits canvas: {nextCreditsCanvasName}");
            return false;
        }

        nextCreditsCanvas.SetActive(true);
        gameObject.SetActive(false);
        return true;
    }

    private string GetNextCreditsCanvasName() {
        if (gameObject.name.Equals("CreditsCanvas", StringComparison.Ordinal)) {
            return "CreditsCanvas Translators 1";
        }

        const string translatorPrefix = "CreditsCanvas Translators ";
        if (!gameObject.name.StartsWith(translatorPrefix, StringComparison.Ordinal)) {
            return null;
        }

        string pageNumberString = gameObject.name.Substring(translatorPrefix.Length);
        if (!int.TryParse(pageNumberString, out int pageNumber)) {
            return null;
        }

        if (pageNumber >= 1 && pageNumber < 6) {
            return $"{translatorPrefix}{pageNumber + 1}";
        }

        return null;
    }

    private static GameObject FindInactiveSceneObject(string objectName) {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()) {
            if (!obj.scene.IsValid()) {
                continue;
            }

            if (obj.name.Equals(objectName, StringComparison.Ordinal)) {
                return obj;
            }
        }

        return null;
    }
}
