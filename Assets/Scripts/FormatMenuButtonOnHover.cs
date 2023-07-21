using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FormatMenuButtonOnHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    public Color textHoverColor;
    public Color imageHoverColor;

    public  TextMeshProUGUI text;
    public  Image           image;
    private Color           _textDefaultColor;
    private Color           _imageDefaultColor;


    private void Start() {
        _textDefaultColor = text.color;
        _imageDefaultColor = image != null ? image.color : Color.black;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        
        StartCoroutine(FormatOn());
    }
    
    private IEnumerator FormatOn() {
        Color textColor = text.color;
        Color imageColor = image!=null?image.color:Color.black;
        float elapsedTime = 0f;
        float lerpDuration = 0.1f; // Total time in seconds for lerping (3 iterations * 0.1 seconds per iteration)

        while (elapsedTime < lerpDuration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration); // Clamp t between 0 and 1
            if (image != null) {
                Color imageLerpColor = Color.Lerp(imageColor, imageHoverColor, t);
                image.color = imageLerpColor;
            }
            Color textLerpColor = Color.Lerp(textColor, textHoverColor, t);
            text.color = textLerpColor;
            yield return null;
        }

        // Ensure the color is set to the final hover color after the lerp is completed
        if (image != null) {
            image.color = imageHoverColor;
        }
        text.color = textHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) {

        StartCoroutine(FormatOff());

    }
    
    private IEnumerator FormatOff() {
        Color textColor = text.color;
        Color imageColor = image!=null?image.color:Color.black;

        float elapsedTime = 0f;
        float lerpDuration = 0.1f; // Total time in seconds for lerping (3 iterations * 0.1 seconds per iteration)

        while (elapsedTime < lerpDuration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration); // Clamp t between 0 and 1
            if (image != null) {
                Color imageLerpColor = Color.Lerp(imageColor, _imageDefaultColor, t);
                image.color = imageLerpColor;
            }
            Color lerpColor = Color.Lerp(textColor, _textDefaultColor, t);
            text.color = lerpColor;
            yield return null;
        }

        // Ensure the color is set to the final default color after the lerp is completed
        if (image != null) {
            image.color = _imageDefaultColor;
        }
        text.color = _textDefaultColor;
    }
}
