using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class AutoNarrativeItem : MonoBehaviour {

    public  string    id;
    public  int       number;
    public  Day       day;
    public  Character character;
    public  bool      phone;
    public  int       offset;

    // private void OnValidate() {
    //     if (generate != lastGen) {
    //         lastGen = generate;
    //         Create();
    //     }
    // }
    [MenuItem("Custom/CreateInstances")]

    public void Create() {
        for (int i = 0; i < number; i++) {
            CreateNarrationItem(i+offset);
        }
    }
    
    private void CreateNarrationItem(int num) {
        NarrationItem newNarrationItem = ScriptableObject.CreateInstance<NarrationItem>();
        newNarrationItem.name = string.Format(id,num);
        newNarrationItem.character = character;
        // Set default properties here if needed
        newNarrationItem.day = day;
        newNarrationItem.phone = phone;

        AssetDatabase.CreateAsset(newNarrationItem, "Assets/Narrative/" + newNarrationItem.name + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
