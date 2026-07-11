using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEngine;

public static class TMPDebugCopyUtility
{
    [MenuItem("CONTEXT/TMP_Text/Copy Debug Snapshot")]
    private static void CopyTmpTextSnapshot(MenuCommand command)
    {
        if (command.context is TMP_Text text)
        {
            CopySnapshot(text, BuildTmpTextSnapshot(text));
        }
    }

    [MenuItem("CONTEXT/TMP_FontAsset/Copy Debug Snapshot")]
    private static void CopyTmpFontAssetSnapshot(MenuCommand command)
    {
        if (command.context is TMP_FontAsset fontAsset)
        {
            CopySnapshot(fontAsset, BuildObjectSnapshot(fontAsset));
        }
    }

    [MenuItem("CONTEXT/TMP_FontAsset/Copy Creation Settings")]
    private static void CopyTmpFontAssetCreationSettings(MenuCommand command)
    {
        if (command.context is TMP_FontAsset fontAsset)
        {
            CopySnapshot(fontAsset, BuildTmpFontAssetCreationSettingsSnapshot(fontAsset));
        }
    }

    [MenuItem("CONTEXT/Font/Copy Debug Snapshot")]
    private static void CopyFontSnapshot(MenuCommand command)
    {
        if (command.context is Font font)
        {
            CopySnapshot(font, BuildObjectSnapshot(font));
        }
    }

    [MenuItem("Tools/Debug/Copy Selected TMP Text Snapshot", true)]
    private static bool ValidateCopySelectedTmpTextSnapshot()
    {
        return Selection.activeObject is TMP_Text;
    }

    [MenuItem("Tools/Debug/Copy Selected TMP Text Snapshot")]
    private static void CopySelectedTmpTextSnapshot()
    {
        CopyTmpTextSnapshot(new MenuCommand(Selection.activeObject));
    }

    [MenuItem("Tools/Debug/Copy Selected TMP Font Asset Snapshot", true)]
    private static bool ValidateCopySelectedTmpFontAssetSnapshot()
    {
        return Selection.activeObject is TMP_FontAsset;
    }

    [MenuItem("Tools/Debug/Copy Selected TMP Font Asset Snapshot")]
    private static void CopySelectedTmpFontAssetSnapshot()
    {
        CopyTmpFontAssetSnapshot(new MenuCommand(Selection.activeObject));
    }

    [MenuItem("Tools/Debug/Copy Selected TMP Font Asset Creation Settings", true)]
    private static bool ValidateCopySelectedTmpFontAssetCreationSettings()
    {
        return Selection.activeObject is TMP_FontAsset;
    }

    [MenuItem("Tools/Debug/Copy Selected TMP Font Asset Creation Settings")]
    private static void CopySelectedTmpFontAssetCreationSettings()
    {
        CopyTmpFontAssetCreationSettings(new MenuCommand(Selection.activeObject));
    }

    [MenuItem("Tools/Debug/Copy Selected Unity Font Snapshot", true)]
    private static bool ValidateCopySelectedFontSnapshot()
    {
        return Selection.activeObject is Font;
    }

    [MenuItem("Tools/Debug/Copy Selected Unity Font Snapshot")]
    private static void CopySelectedFontSnapshot()
    {
        CopyFontSnapshot(new MenuCommand(Selection.activeObject));
    }

    [MenuItem("Assets/Debug/Copy Selected Unity Font Snapshot", true)]
    private static bool ValidateCopySelectedFontSnapshotFromAssets()
    {
        return Selection.activeObject is Font;
    }

    [MenuItem("Assets/Debug/Copy Selected Unity Font Snapshot")]
    private static void CopySelectedFontSnapshotFromAssets()
    {
        CopySelectedFontSnapshot();
    }

    [MenuItem("Assets/Debug/Copy Selected TMP Font Asset Snapshot", true)]
    private static bool ValidateCopySelectedTmpFontAssetSnapshotFromAssets()
    {
        return Selection.activeObject is TMP_FontAsset;
    }

    [MenuItem("Assets/Debug/Copy Selected TMP Font Asset Snapshot")]
    private static void CopySelectedTmpFontAssetSnapshotFromAssets()
    {
        CopySelectedTmpFontAssetSnapshot();
    }

    [MenuItem("Assets/Debug/Copy Selected TMP Font Asset Creation Settings", true)]
    private static bool ValidateCopySelectedTmpFontAssetCreationSettingsFromAssets()
    {
        return Selection.activeObject is TMP_FontAsset;
    }

    [MenuItem("Assets/Debug/Copy Selected TMP Font Asset Creation Settings")]
    private static void CopySelectedTmpFontAssetCreationSettingsFromAssets()
    {
        CopySelectedTmpFontAssetCreationSettings();
    }

    private static void CopySnapshot(UnityEngine.Object target, string snapshot)
    {
        EditorGUIUtility.systemCopyBuffer = snapshot;
        Debug.Log($"Copied debug snapshot for {target.name} ({target.GetType().Name}) to clipboard.", target);
    }

    private static string BuildTmpTextSnapshot(TMP_Text text)
    {
        var extraLines = new List<string>
        {
            $"hierarchyPath: {GetHierarchyPath(text.transform)}",
            $"isPlaying: {Application.isPlaying}",
            $"activeInHierarchy: {text.gameObject.activeInHierarchy}",
            $"text: {EscapeMultiline(text.text)}",
            $"fontAsset: {FormatObjectReference(text.font)}",
            $"fontSharedMaterial: {FormatObjectReference(text.fontSharedMaterial)}",
            $"fontMaterial: {FormatObjectReference(text.fontMaterial)}",
            $"color: {text.color}",
            $"fontSize: {text.fontSize}",
            $"fontStyle: {text.fontStyle}",
            $"alignment: {text.alignment}",
            $"enableWordWrapping: {text.enableWordWrapping}",
            $"overflowMode: {text.overflowMode}",
            $"lineSpacing: {text.lineSpacing}",
            $"characterSpacing: {text.characterSpacing}",
            $"wordSpacing: {text.wordSpacing}",
            $"paragraphSpacing: {text.paragraphSpacing}",
            $"isTextOverflowing: {text.isTextOverflowing}",
            $"preferredWidth: {text.preferredWidth}",
            $"preferredHeight: {text.preferredHeight}",
            $"renderedWidth: {text.renderedWidth}",
            $"renderedHeight: {text.renderedHeight}",
            $"bounds: center={text.bounds.center} size={text.bounds.size}",
            $"rectTransform: {FormatRectTransform(text.rectTransform)}"
        };

        return BuildObjectSnapshot(text, extraLines);
    }

    private static string BuildObjectSnapshot(UnityEngine.Object target, List<string> extraLines = null)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"name: {target.name}");
        builder.AppendLine($"type: {target.GetType().FullName}");
        builder.AppendLine($"assetPath: {AssetDatabase.GetAssetPath(target)}");
        builder.AppendLine($"instanceId: {target.GetInstanceID()}");
        builder.AppendLine();

        if (extraLines != null && extraLines.Count > 0)
        {
            builder.AppendLine("[summary]");
            foreach (string line in extraLines)
            {
                builder.AppendLine(line);
            }
            builder.AppendLine();
        }

        builder.AppendLine("[editorJson]");
        builder.AppendLine(EditorJsonUtility.ToJson(target, true));
        builder.AppendLine();

        builder.AppendLine("[serializedProperties]");
        AppendSerializedProperties(builder, target);
        return builder.ToString();
    }

    private static string BuildTmpFontAssetCreationSettingsSnapshot(TMP_FontAsset fontAsset)
    {
        var so = new SerializedObject(fontAsset);
        var builder = new StringBuilder();
        builder.AppendLine($"name: {fontAsset.name}");
        builder.AppendLine($"type: {fontAsset.GetType().FullName}");
        builder.AppendLine($"assetPath: {AssetDatabase.GetAssetPath(fontAsset)}");
        builder.AppendLine();
        builder.AppendLine("[creationSettings]");
        AppendPropertyIfFound(builder, so, "m_SourceFontFile");
        AppendPropertyIfFound(builder, so, "m_AtlasPopulationMode");
        AppendPropertyIfFound(builder, so, "m_AtlasWidth");
        AppendPropertyIfFound(builder, so, "m_AtlasHeight");
        AppendPropertyIfFound(builder, so, "m_AtlasPadding");
        AppendPropertyIfFound(builder, so, "m_AtlasRenderMode");
        AppendPropertyIfFound(builder, so, "m_IsMultiAtlasTexturesEnabled");
        AppendPropertyIfFound(builder, so, "m_ClearDynamicDataOnBuild");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.sourceFontFileGUID");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.pointSizeSamplingMode");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.pointSize");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.padding");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.packingMode");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.atlasWidth");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.atlasHeight");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.characterSetSelectionMode");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.characterSequence");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.fontStyle");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.fontStyleModifier");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.renderMode");
        AppendPropertyIfFound(builder, so, "m_CreationSettings.includeFontFeatures");
        return builder.ToString();
    }

    private static void AppendPropertyIfFound(StringBuilder builder, SerializedObject serializedObject, string propertyPath)
    {
        SerializedProperty property = serializedObject.FindProperty(propertyPath);
        if (property != null)
        {
            builder.AppendLine($"{propertyPath} ({property.propertyType}) = {FormatPropertyValue(property)}");
        }
        else
        {
            builder.AppendLine($"{propertyPath} = <missing>");
        }
    }

    private static void AppendSerializedProperties(StringBuilder builder, UnityEngine.Object target)
    {
        var serializedObject = new SerializedObject(target);
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            builder.AppendLine($"{iterator.propertyPath} ({iterator.propertyType}) = {FormatPropertyValue(iterator)}");
            enterChildren = false;
        }
    }

    private static string FormatPropertyValue(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue.ToString();
            case SerializedPropertyType.Boolean:
                return property.boolValue.ToString();
            case SerializedPropertyType.Float:
                return property.floatValue.ToString();
            case SerializedPropertyType.String:
                return EscapeMultiline(property.stringValue);
            case SerializedPropertyType.Color:
                return property.colorValue.ToString();
            case SerializedPropertyType.ObjectReference:
                return FormatObjectReference(property.objectReferenceValue);
            case SerializedPropertyType.LayerMask:
                return property.intValue.ToString();
            case SerializedPropertyType.Enum:
                return $"{property.enumDisplayNames[property.enumValueIndex]} ({property.enumValueIndex})";
            case SerializedPropertyType.Vector2:
                return property.vector2Value.ToString();
            case SerializedPropertyType.Vector3:
                return property.vector3Value.ToString();
            case SerializedPropertyType.Vector4:
                return property.vector4Value.ToString();
            case SerializedPropertyType.Rect:
                return property.rectValue.ToString();
            case SerializedPropertyType.ArraySize:
                return property.intValue.ToString();
            case SerializedPropertyType.Character:
                return property.intValue.ToString();
            case SerializedPropertyType.AnimationCurve:
                return property.animationCurveValue != null ? property.animationCurveValue.ToString() : "null";
            case SerializedPropertyType.Bounds:
                return property.boundsValue.ToString();
            case SerializedPropertyType.Gradient:
                return "<gradient>";
            case SerializedPropertyType.Quaternion:
                return property.quaternionValue.eulerAngles.ToString();
            case SerializedPropertyType.ExposedReference:
                return FormatObjectReference(property.exposedReferenceValue);
            case SerializedPropertyType.FixedBufferSize:
                return property.fixedBufferSize.ToString();
            case SerializedPropertyType.Vector2Int:
                return property.vector2IntValue.ToString();
            case SerializedPropertyType.Vector3Int:
                return property.vector3IntValue.ToString();
            case SerializedPropertyType.RectInt:
                return property.rectIntValue.ToString();
            case SerializedPropertyType.BoundsInt:
                return property.boundsIntValue.ToString();
            case SerializedPropertyType.ManagedReference:
                return property.managedReferenceFullTypename;
            case SerializedPropertyType.Hash128:
                return property.hash128Value.ToString();
            case SerializedPropertyType.Generic:
                return property.isArray ? $"Array(size={property.arraySize})" : "<generic>";
            default:
                return "<unsupported>";
        }
    }

    private static string FormatObjectReference(UnityEngine.Object value)
    {
        if (value == null)
        {
            return "null";
        }

        string path = AssetDatabase.GetAssetPath(value);
        if (string.IsNullOrEmpty(path))
        {
            return $"{value.name} ({value.GetType().Name})";
        }

        return $"{value.name} ({value.GetType().Name}) @ {path}";
    }

    private static string FormatRectTransform(RectTransform rectTransform)
    {
        if (rectTransform == null)
        {
            return "null";
        }

        return $"anchorMin={rectTransform.anchorMin}, anchorMax={rectTransform.anchorMax}, pivot={rectTransform.pivot}, sizeDelta={rectTransform.sizeDelta}, anchoredPosition={rectTransform.anchoredPosition}";
    }

    private static string GetHierarchyPath(Transform transform)
    {
        if (transform == null)
        {
            return string.Empty;
        }

        var names = new Stack<string>();
        Transform current = transform;
        while (current != null)
        {
            names.Push(current.name);
            current = current.parent;
        }

        return string.Join("/", names);
    }

    private static string EscapeMultiline(string value)
    {
        if (value == null)
        {
            return "null";
        }

        var builder = new StringBuilder(value.Length);
        foreach (char ch in value)
        {
            switch (ch)
            {
                case '\r':
                    builder.Append("\\r");
                    break;
                case '\n':
                    builder.Append("\\n");
                    break;
                case '\t':
                    builder.Append("\\t");
                    break;
                case '\\':
                    builder.Append("\\\\");
                    break;
                default:
                    if (ch < 32 || ch > 126)
                    {
                        builder.Append("\\u");
                        builder.Append(((int)ch).ToString("X4", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        builder.Append(ch);
                    }
                    break;
            }
        }

        return builder.ToString();
    }
}
