using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormatMenuButtonOnHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    public  Color           hover;
    public  TextMeshProUGUI text;
    private Color           defaultColor;


    private void Start() {
        defaultColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        
        StartCoroutine(FormatOn());
    }
    
    private IEnumerator FormatOn() {
        Color color = text.color;
        float elapsedTime = 0f;
        float lerpDuration = 0.1f; // Total time in seconds for lerping (3 iterations * 0.1 seconds per iteration)

        while (elapsedTime < lerpDuration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration); // Clamp t between 0 and 1
            Color lerpColor = Color.Lerp(color, hover, t);
            text.color = lerpColor;
            yield return null;
        }

        // Ensure the color is set to the final hover color after the lerp is completed
        text.color = hover;
    }

    public void OnPointerExit(PointerEventData eventData) {

        StartCoroutine(FormatOff());

    }
    
    private IEnumerator FormatOff() {
        Color color = text.color;
        float elapsedTime = 0f;
        float lerpDuration = 0.1f; // Total time in seconds for lerping (3 iterations * 0.1 seconds per iteration)

        while (elapsedTime < lerpDuration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration); // Clamp t between 0 and 1
            Color lerpColor = Color.Lerp(color, defaultColor, t);
            text.color = lerpColor;
            yield return null;
        }

        // Ensure the color is set to the final default color after the lerp is completed
        text.color = defaultColor;
    }
}
