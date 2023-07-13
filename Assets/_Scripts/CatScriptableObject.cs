using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cats", menuName ="Cats/Cat")]
public class CatScriptableObject : ScriptableObject
{
    public string catName;
    public int catAge;
    public Sprite catImage;
}
