using UnityEditor;
using UnityEngine;

namespace Editor.AttributesWeights
{
    [CustomEditor(typeof(CardStatWeight))]
    public class CardStatWeightInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Stat Weight Data Setup Window", EditorStyles.miniButton))
            {
                WeightDataEditorWindow instance = EditorWindow.GetWindow<WeightDataEditorWindow>();
                instance.Show();
            }
            GUI.enabled = false;
            base.OnInspectorGUI();
        }
    }
}