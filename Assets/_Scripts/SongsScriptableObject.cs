using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Song", menuName = "Songs/Song")]
public class SongsScriptableObject : ScriptableObject
{
    public string songName;
    public string songAuthor;
   /* AudioClip track;
    Time duration;
    Sprite Icon; */
}
