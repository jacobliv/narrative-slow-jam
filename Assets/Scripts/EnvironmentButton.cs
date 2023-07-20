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
    }

    private void PlaySfx() {
        audioSource.Stop();
        audioSource.clip = sounds[Random.Range(0, sounds.Count - 1)];
        Debug.Log(audioSource.clip.name);
        audioSource.Play();
    }
}
