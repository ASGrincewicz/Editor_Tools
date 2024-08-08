using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(CardStat))]
    public class CardStatPropertyDrawer : PropertyDrawer
    {

        private const string DIVIDER = "_____________________";
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statName").stringValue}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statValue").intValue}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statDescription").stringValue}",EditorStyles.wordWrappedMiniLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{DIVIDER}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}