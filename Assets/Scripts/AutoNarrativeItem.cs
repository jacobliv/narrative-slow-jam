using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static System.Globalization.CultureInfo;

public class AutoNarrativeItem : MonoBehaviour {

    public string          id;
    public int             number;
    public Day             day;
    public Character       character;
    public bool            phone;
    public int             offset;
    public List<Character> characters;
    [Header("CSV File")]
    public TextAsset csvFile; // Reference to your CSV file in the Unity project.
    // private void OnValidate() {
    //     if (generate != lastGen) {
    //         lastGen = generate;
    //         Create();
    //     }
    // }
    [MenuItem("Custom/CreateInstances")]

    public void Create() {
        // for (int i = 0; i < number; i++) {
        //     CreateNarrationItem(i+offset);
        // }
        LoadDataFromCSV();
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
    
    
   

    public void LoadDataFromCSV() {
        if (csvFile == null) {
            Debug.LogError("CSV file reference missing!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        int startIndex = 0;


        for (int i = 1; i < lines.Length; i++) {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) {
                continue;
            }

            string[] fields = line.Split(',');

            NarrationItem instance = ScriptableObject.CreateInstance<NarrationItem>();
            instance.day =Day.One;
            if (fields[1] != "") {
                Character c = characters.Find((c) => c.name == fields[1]);
                if (c == null) {
                    Debug.Log("Unable to find " + fields[1]);
                    continue;
                }

                instance.character = c;
            }
            
            instance.line = line.Replace($"{fields[0]},{fields[1]},",""); 
            
            AssetDatabase.CreateAsset(instance, "Assets/Narrative/Generated/" + fields[0] + ".asset");
        }
    }
}
