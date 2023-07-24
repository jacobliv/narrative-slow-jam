
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class NarrativeHistory : MonoBehaviour {
    [SerializeField]
    public Dictionary<string, CharacterHistory> narrativeHistory = new();
    public List<NarrationItem> linearHistory = new();

    public void AddNarrativeHistory(NarrationItem currentNarrativeItem,int option) {
        Character character = currentNarrativeItem.character;
        narrativeHistory[character!=null ?character.name: "Narrator"]=new CharacterHistory().AddHistory(currentNarrativeItem.next[option].shortenedLine);

    }
}

[Serializable]
public class CharacterHistory {
    
    [SerializeField]
    public List<string> characterChoices = new();

    public CharacterHistory AddHistory(string choice) {
        characterChoices.Add(choice);
        return this;
    }
}