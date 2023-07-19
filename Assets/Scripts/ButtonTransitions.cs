using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTransitions : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private Button            _button;
    [SerializeField] private ButtonTransitions _otherButton;
    [SerializeField] private TextMeshProUGUI   text;
    [SerializeField] private Color             hoverColor;
    [SerializeField] private Color             selectedColor;
    private                  Color             defaultColor;
    private                  bool              _selected;


    public void Reset() {
        _selected = false;
        text.color = defaultColor;

    }

    private void Start() {
        defaultColor = text.color;
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        _otherButton.Reset();
        text.color = selectedColor;
        _selected = true;
        text.fontStyle = FontStyles.Bold;

    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(_selected) return;
        text.color = hoverColor;
        text.fontStyle = FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(_selected) return;
        text.color = defaultColor;
        text.fontStyle = FontStyles.Normal;

    }
    
}
