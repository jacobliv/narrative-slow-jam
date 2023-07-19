using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CurrentCharacterSprite))]
public class CurrentCharacterSpriteDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty spriteProperty = property.FindPropertyRelative("sprite");

        // Find the parent SerializedObject to access the NarrativeItem
        SerializedObject parentObject = property.serializedObject;

        // Get the CharacterScriptableObject property from the parent object
        SerializedProperty characterProperty = parentObject.FindProperty("character");

        // Ensure the parent object and character property are valid
        if (parentObject != null && characterProperty != null) {
            // Get the selected character from the CharacterScriptableObject property
            Character character = (Character)characterProperty.objectReferenceValue;

            // If the character is valid and has sprites, show a dropdown with the available sprites
            if (character != null && character.sprites != null && character.sprites.Count > 0) {
                int spriteIndex = GetSpriteIndex(character, spriteProperty);
                int newSpriteIndex = EditorGUI.Popup(position, "Sprite", spriteIndex, GetSpriteNames(character));
                spriteProperty.objectReferenceValue = character.sprites[newSpriteIndex];
            }
            else {
                EditorGUI.LabelField(position, "No character or sprites available.");
            }
        }

        EditorGUI.EndProperty();
    }

    // Get the index of the current sprite in the character's sprite list
    private int GetSpriteIndex(Character character, SerializedProperty spriteProperty) {
        if (character == null || character.sprites == null || character.sprites.Count == 0)
            return 0;

        Sprite sprite = (Sprite)spriteProperty.objectReferenceValue;
        return Mathf.Max(character.sprites.IndexOf(sprite), 0);
    }

    // Get the names of the sprites in the character's sprite list
    private string[] GetSpriteNames(Character character) {
        if (character == null || character.sprites == null || character.sprites.Count == 0)
            return new string[] { "No sprites available" };

        string[] spriteNames = new string[character.sprites.Count];
        for (int i = 0; i < character.sprites.Count; i++) {
            spriteNames[i] = character.sprites[i].name;
        }

        return spriteNames;
    }
}
