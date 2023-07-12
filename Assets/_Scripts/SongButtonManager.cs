using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongButtonManager : MonoBehaviour
{
    [SerializeField] SongsScriptableObject song;
    [SerializeField] TMP_Text songName, author, duration;
    public AudioManagerScript audioMan;

    void Start()
    {
        songName.text = song.songName;
        author.text = song.songAuthor;
        duration.text = "3:23";

    }

    public void SongChosen()
    {
        audioMan.PlayTrack(song.track);
    }
}
