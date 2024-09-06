using Editor.CardData;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.CardEditor
{
    [CustomPropertyDrawer(typeof(CardStat))]
    public class CardStatPropertyDrawer : PropertyDrawer
    {

        private const string DIVIDER = "_____________________";
        private StatNames _statName;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            SerializedProperty statNameProp = property.FindPropertyRelative("_statName");
            EditorGUILayout.LabelField(((StatNames)statNameProp.enumValueIndex).GetDescription(), EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statValue").intValue}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statDescription").stringValue}",EditorStyles.wordWrappedMiniLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{DIVIDER}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}