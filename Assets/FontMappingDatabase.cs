using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FontMappingDatabase", menuName = "ScriptableObjects/FontMappingDatabase", order = 1)]

public class FontMappingDatabase : ScriptableObject {
    public List<FontLanguageMapping> mappings;
}
