using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LanguageDatabase", menuName = "ScriptableObjects/LanguageDatabase", order = 1)]
public class LanguageDatabase : ScriptableObject {
    public List<Language> languages;
}