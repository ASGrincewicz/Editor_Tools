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
            window.position = new Rect(50, 50, 400, 300);
            window.Show();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            // Draw Button Area
            // Draw Keyword List Area
        }

        private void SetupAreaRects()
        {
            _mainAreaRect = new Rect(20,5, 400, 300);
        }
    }
}