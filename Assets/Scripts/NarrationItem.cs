using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Narrative", menuName = "Narrative/Narrative Item")]
public class NarrationItem : ScriptableObject{
    [Tooltip("Day the narration occurs")]
    public Day                 day;
    [Tooltip("Character who is speaking")]
    public Character character;
    [Tooltip("Sounds that play in order from the beginning of the narration")]
    public List<AudioClip> sounds;
    [TextArea,Tooltip("Text that is spoken by the character")]
    public string line;
    [Tooltip("Next Narrative item. 1 or more")]
    public List<NextNarrative> next;
    [Tooltip("Image to be displayed behind the characters")]
    public Image background;
    [Tooltip("The time of day this occurs")]
    public string time;

}

[Serializable]
public class NextNarrative {
    [SerializeField, Tooltip("What triggers the next narration.")]
    public string        button;
    [SerializeField, Tooltip("The next narration to occur")]
    public NarrationItem narrativeItem;
}

public enum Day {
    Pre,
    One,
    Two,
    Three,
    Post
}