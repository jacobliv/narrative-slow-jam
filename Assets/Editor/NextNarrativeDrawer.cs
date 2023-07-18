using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NextNarrative))]
public class NextNarrativeDrawer : PropertyDrawer
{
    private ButtonManager buttonManager;
    private string[] buttonNames;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        Rect buttonPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect otherPosition = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, EditorGUIUtility.singleLineHeight);

        buttonManager = Object.FindObjectOfType<ButtonManager>();

        if (buttonManager == null) {
            EditorGUI.HelpBox(position, "ButtonManager not found in the scene.", MessageType.Warning);
            EditorGUI.EndProperty();
            return;
        }

        SerializedProperty buttonProperty = property.FindPropertyRelative("button");
        int selectedIndex = GetSelectedButtonIndex(buttonProperty.stringValue);

        // Create an array of button names to display in the popup
        buttonNames = GetButtonNames();

        // Display the button name in the folded-up dropdown
        string selectedButtonName = selectedIndex >= 0 && selectedIndex < buttonNames.Length ? buttonNames[selectedIndex] : "None";
        var pos = EditorGUI.PrefixLabel(buttonPosition, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Button: "));
        pos.x -= pos.width/2;
        selectedIndex = EditorGUI.Popup(EditorGUI.IndentedRect(pos), selectedIndex, buttonNames);

        // Update the serialized property with the selected button
        if (selectedIndex >= 0 && selectedIndex < buttonNames.Length) {
            buttonProperty.stringValue = buttonNames[selectedIndex];
        }
        

        // Draw the other field
        EditorGUI.PropertyField(otherPosition, property.FindPropertyRelative("narrativeItem"), GUIContent.none);

        EditorGUI.EndProperty();
    }

    private int GetSelectedButtonIndex(string buttonName)
    {
        for (int i = 0; i < buttonManager.buttons.Count; i++)
        {
            if (buttonManager.buttons[i].name == buttonName)
            {
                return i;
            }
        }

        return -1;
    }

    private string[] GetButtonNames()
    {
        string[] names = new string[buttonManager.buttons.Count];
        for (int i = 0; i < buttonManager.buttons.Count; i++)
        {
            names[i] = buttonManager.buttons[i].name;
        }

        return names;
    }
}
