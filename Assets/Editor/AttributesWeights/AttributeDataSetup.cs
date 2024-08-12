using UnityEditor;
using UnityEngine;

namespace Editor.AttributesWeights
{
    public class AttributeDataSetup : EditorWindow
    {

        public AttributeSettings attributeSettings;
        [MenuItem("Tools/Attribute Data Setup")]
        private static void ShowWindow()
        {
            AttributeDataSetup window = GetWindow<AttributeDataSetup>();
            window.titleContent = new GUIContent("Attribute Data Setup");
            window.Show();
        }

        private void CreateGUI()
        {
            
        }
    }
}