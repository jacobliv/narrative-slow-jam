using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlayTrack(AudioClip song)
    {
        audioSource.clip = song;
        audioSource.Play();
    }
}
