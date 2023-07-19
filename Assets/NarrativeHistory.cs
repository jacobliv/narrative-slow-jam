
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class NarrativeHistory : MonoBehaviour {
    [SerializeField]
    public Dictionary<string, CharacterHistory> narrativeHistory = new() {
        { "bob", new CharacterHistory().AddHistory("My Choice")},
        { "stella", new CharacterHistory().AddHistory("My a")},

    };
    
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