using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.CardData
{
    [CustomPropertyDrawer(typeof(CardStat))]
    public class CardStatPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            SerializedProperty statNameProp = property.FindPropertyRelative("_statName");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(((StatNames)statNameProp.enumValueIndex).GetDescription(), EditorStyles.boldLabel, GUILayout.Width(50), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statValue").intValue}",EditorStyles.boldLabel, GUILayout.Width(20), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statDescription").stringValue}",EditorStyles.wordWrappedMiniLabel, GUILayout.Width(100), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}