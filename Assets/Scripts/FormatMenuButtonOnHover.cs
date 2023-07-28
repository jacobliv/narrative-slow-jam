using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class FormatMenuButtonOnHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler {
    public Color textHoverColor;
    public Color imageHoverColor;
    
    public bool  underline;

    public TextMeshProUGUI text;
    public Image           image;
    public Image           secondImage;

    private Color     _textDefaultColor;
    private Color     _imageDefaultColor;
    private Color     _secondImageDefaultColor;
    private bool      started;
    public  Texture2D pointer;
    public  Texture2D arrow;
    private bool      inside;


    private void OnDisable() {
        if (text != null && started) {
            text.color = _textDefaultColor;

        }
        if (image != null && started) {
            image.color = _imageDefaultColor;

        }
        if (secondImage != null&& started) {
            secondImage.color =_secondImageDefaultColor;
        }

        if (!inside) {
            Cursor.SetCursor(arrow,Vector2.zero,CursorMode.Auto);

        }
        Debug.Log($"Reset {name} -- {transform.parent.name}" );

        // Cursor.SetCursor(CursorType.Hand, Vector2.zero, CursorMode.Auto); // Changes the cursor to the Hand cursor
    }
    
    

    private void Start() {
        started = true;
        _textDefaultColor = text != null ? text.color:Color.red;
        _imageDefaultColor = image != null ? image.color : Color.red;
        _secondImageDefaultColor=secondImage != null ? secondImage.color : Color.red;

    }

    public void OnPointerEnter(PointerEventData eventData) {
        inside = true;
        Cursor.SetCursor(pointer,new Vector2(pointer.width/3f,0),CursorMode.Auto);
        StartCoroutine(FormatOn());
    }
    
    private IEnumerator FormatOn() {
        Color textColor = text!=null ? text.color:Color.black;
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
            if (text != null) {
                Color textLerpColor = Color.Lerp(textColor, textHoverColor, t);
                text.color = textLerpColor;
                
            }

            if (secondImage != null) {
                Color textLerpColor = Color.Lerp(textColor, textHoverColor, t);
                secondImage.color = textLerpColor;
            }
            
            yield return null;
        }

        // Ensure the color is set to the final hover color after the lerp is completed
        if (image != null) {
            image.color = imageHoverColor;
        }

        if (text != null) {
            text.color = textHoverColor;
            if (secondImage != null) {
                
                secondImage.color = textHoverColor;
            }
            if (underline) {
                text.fontStyle = FontStyles.Underline;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        Cursor.SetCursor(arrow,Vector2.zero,CursorMode.Auto);
        inside = false;
        StartCoroutine(FormatOff());

    }
    
    private IEnumerator FormatOff() {
        Color textColor = text!=null?text.color:Color.black;
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
            if (text != null) {
                Color lerpColor = Color.Lerp(textColor, _textDefaultColor, t);
                text.color = lerpColor;
            }
            if (secondImage != null) {
                Color textLerpColor = Color.Lerp(textColor, _textDefaultColor, t);
                secondImage.color = textLerpColor;
            }
            yield return null;
        }

        // Ensure the color is set to the final default color after the lerp is completed
        if (image != null) {
            image.color = _imageDefaultColor;
        }
        if (text != null) {
            text.color = _textDefaultColor;
            text.fontStyle = FontStyles.Normal;
            if (secondImage != null) {
                
                secondImage.color = _secondImageDefaultColor;
            }
        }
    }

    public void OnPointerMove(PointerEventData eventData) {
        inside = true;
    }
}
