using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Song", menuName = "Songs/Song")]
public class SongsScriptableObject : ScriptableObject
{
    string songName;
    string songAuthor;
    AudioClip track;
    Time duration;
    Sprite Icon;
}
