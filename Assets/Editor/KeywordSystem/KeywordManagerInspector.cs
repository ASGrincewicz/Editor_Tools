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
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Open Keyword Editor", EditorStyles.toolbarButton))
            {
                KeywordEditorWindow instance = EditorWindow.GetWindow<KeywordEditorWindow>();
                instance.Show();
            }

            if (GUILayout.Button("Enable/Disable GUI",EditorStyles.toolbarButton))
            {
                _isGUIEnabled = !_isGUIEnabled;
            }
            GUILayout.EndHorizontal();
            GUI.enabled = _isGUIEnabled;
            base.OnInspectorGUI();
        }
    }
}