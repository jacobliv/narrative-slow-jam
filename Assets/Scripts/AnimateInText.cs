using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimateInText : MonoBehaviour {
    public  TMP_Text  textMeshPro;
    public  float     delayBetweenCharacters = 0.01f;
    private Coroutine animate;

    private void OnValidate() {
        if (Application.isPlaying) {
            AnimateText();
        }
    }

    private void Start() {
        AnimateText();
    }

    public void AnimateText() {
        if(animate != null) {
            StopCoroutine(animate);
        }   
        animate=StartCoroutine(AnimateTextCoroutine());
    }

    private IEnumerator AnimateTextCoroutine() {
        textMeshPro.maxVisibleCharacters = 0;

        yield return null; // Wait for one frame before starting the animation

        float elapsedTime = 0f;

        while (elapsedTime < delayBetweenCharacters * textMeshPro.text.Length)
        {
            elapsedTime += Time.deltaTime;
            int charactersToShow = Mathf.FloorToInt(elapsedTime / delayBetweenCharacters);
            textMeshPro.maxVisibleCharacters = charactersToShow;

            yield return null;
        }

        // Ensure all characters are visible at the end of the animation
        textMeshPro.maxVisibleCharacters = textMeshPro.text.Length;
    }
}
