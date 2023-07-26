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

    public CharacterEnum characterArt;
    [Tooltip("Choose if this narrative line takes place on your phone")]
    public bool phone;
    
    public bool shopSelection;
    public bool internalThought;
    public bool physicalInteraction;

    [Tooltip("Sounds that play in order from the beginning of the narration")]
    public List<AudioClip> sounds;
    [TextArea,Tooltip("Text that is spoken by the character")]
    public string line;
    [Tooltip("Next Narrative item. 1 or more")]
    public List<NextNarrative> next;

    [Tooltip("Image to be displayed behind the characters")]
    public Sprite background;
    [Tooltip("The time of day this occurs")]
    public string time;

}

public enum CharacterEnum {
    Rob,
    Dmi,
    Cassiopeia,
    BOO8,
    Aiyana,
    Calder,
    Flubber,
    Kkili,
    RemRom,
    Zerua,
    Crust,
    None
}

[Serializable]
public class CurrentCharacterSprite {
    [SerializeField] public Sprite sprite;
}

[Serializable]
public class NextNarrative {
    public NextNarrative(bool choiceDependent, string previousChoice, string button, NarrationItem narrativeItem, string shortenedLine) {
        this.choiceDependent = choiceDependent;
        this.previousChoice = previousChoice;
        this.button = button;
        this.narrativeItem = narrativeItem;
        this.shortenedLine = shortenedLine;
    }

    [SerializeField,Tooltip("Dependent on previous choice")]
    public bool choiceDependent;

    [SerializeField, Tooltip("The choice that needs to have been made to go this path. Needs to be the same as the shortened line from whichever choice was being made")]
    public string previousChoice;
    [SerializeField, Tooltip("What triggers the next narration.")]
    public string        button;
    [SerializeField, Tooltip("The next narration to occur")]
    public NarrationItem narrativeItem;
    [SerializeField, Tooltip("The shortened line if needed to be displayed for a choice. Can be empty")]
    public string shortenedLine;
}

public enum Day {
    Pre,
    One,
    Two,
    Three,
    Post
}
