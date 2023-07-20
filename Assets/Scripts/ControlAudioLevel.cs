using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControlAudioLevel : MonoBehaviour {
    public AudioMixer mixer;

    public                   Slider slider;
    private float  multiplier;
    [SerializeField] private string parameterName;


    void Start() {
        slider.onValueChanged.AddListener(ChangeVolume);
        ChangeVolume(slider.value);
    }

    // Update is called once per frame
    void ChangeVolume(float value) {
        if (value < .001) value = .001f;
        mixer.SetFloat(parameterName, Mathf.Log10(value*1.2f)*76.8f);
    }
}
