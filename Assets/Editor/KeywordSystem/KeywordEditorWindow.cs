using UnityEditor;
using UnityEngine;

namespace Editor.KeywordSystem
{
    public class KeywordEditorWindow : EditorWindow
    {
        
        // GUI variables
        private Rect _mainAreaRect;
        private Rect _buttonAreaRect;
        private Rect _keywordListAreaRect;
        
        
        [MenuItem("Tools/Keyword Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<KeywordEditorWindow>();
            window.titleContent = new GUIContent("Keyword Editor");
            window.position = new Rect(50, 50, 200, 300);
            window.Show();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawMainArea();
            DrawButtonArea();
            DrawKeywordListArea();
        }

        private void SetupAreaRects()
        {
            _mainAreaRect = new Rect(20,5, 200, position.height * 0.40f);
            _buttonAreaRect = new Rect(20, _mainAreaRect.height + 10, 200, position.height * 0.10f);
            _keywordListAreaRect = new Rect(20, _buttonAreaRect.height + 10, 200, position.height * 0.50f);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(_mainAreaRect);
            GUILayout.TextField("Keyword Name:", GUI.skin.textArea);
            //EditorGUI.IntField("Keyword Value:");
            // Enum popup field
            GUILayout.EndArea();
        }

        private void DrawButtonArea()
        {
            GUILayout.BeginArea(_buttonAreaRect);
            GUILayout.Button("Save");
            GUILayout.EndArea();
        }

        private void DrawKeywordListArea()
        {
            GUILayout.BeginArea(_keywordListAreaRect);
            // Get list of all current keywords
            GUILayout.EndArea();
        }
    }
}