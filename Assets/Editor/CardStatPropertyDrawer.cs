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
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label, EditorStyles.label);
            
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect valueRect = new Rect(position.x + 20, position.y, 110, position.height/2);
            Rect descriptionRect = new Rect(position.x, position.y +2, position.width, position.height/2);
            EditorGUILayout.LabelField($"{property.FindPropertyRelative("_statDescription").stringValue}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField($"{DIVIDER}",EditorStyles.boldLabel, GUILayout.Width(200), GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("_statValue"));
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}