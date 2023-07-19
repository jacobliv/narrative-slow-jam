
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class NarrativeHistory : MonoBehaviour {
    [SerializeField]
    public List<HistoryItem> narrativeHistory = new() {new HistoryItem("bob",new CharacterHistory().AddHistory("My choice").AddHistory("Another Choice")) };

    
}

[Serializable]
public class HistoryItem {
    [SerializeField] public string           name;
    [SerializeField] public CharacterHistory characterHistory;
    public HistoryItem(string name, CharacterHistory characterHistory) {
        this.name = name;
        this.characterHistory = characterHistory;
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