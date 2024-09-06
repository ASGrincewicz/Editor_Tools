using UnityEditor;
using UnityEngine;

namespace Editor.KeywordSystem
{
    [CustomEditor(typeof(KeywordManager))]
    public class KeywordManagerInspector : UnityEditor.Editor
    {
        private bool _isGUIEnabled = false;
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Keyword Editor"))
            {
                KeywordEditorWindow instance = EditorWindow.GetWindow<KeywordEditorWindow>();
                instance.Show();
            }

            if (GUILayout.Button("Enable/Disable GUI"))
            {
                _isGUIEnabled = !_isGUIEnabled;
            }
            GUI.enabled = _isGUIEnabled;
            base.OnInspectorGUI();
        }
    }
}