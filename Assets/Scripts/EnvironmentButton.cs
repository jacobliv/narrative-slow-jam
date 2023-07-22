using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentButton : MonoBehaviour {
    [SerializeField] private List<AudioClip> sounds;
    [SerializeField] private AudioSource     audioSource;
    [SerializeField] private Button          button;
    void Start() {
        button.onClick.AddListener(PlaySfx);
        Debug.Log("Added listener: " + transform.parent.name);
    }

    public void PlaySfx() {
        Debug.Log("Attempting to play sound");
        audioSource.Stop();
        audioSource.clip = sounds[Random.Range(0, sounds.Count)];
        Debug.Log(audioSource.clip.name);
        audioSource.Play();
    }
}
