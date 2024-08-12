using System.Collections.Generic;
using Editor.CardEditor;
using UnityEditor;
using UnityEngine;

namespace Editor.AttributesWeights
{
    public class AttributeDataSetup : EditorWindow
    {
        public AttributeSettings attributeSettings;
        [MenuItem("Tools/Utilities/Attribute Data Setup")]
        private static void ShowWindow()
        {
            AttributeDataSetup window = GetWindow<AttributeDataSetup>();
            window.titleContent = new GUIContent("Attribute Data Setup");
            window.position = new Rect(50, 50, 600, 600);
            window.Show();
        }

        private void OnGUI()
        {
            // Get settings asset for loading and saving data.
            
            // SetupAreaRects
            // Draw Labels
        }
        
        // Setup Area Rects
        // Stat input area
        //   66% of window width.
        //   75% of window height.
        // Button Area
        //   33% of window width.
        //   75% of window height.
        //  Bottom Area
        //    contains settings object field and message area
        
        // DrawCardTypeEnumPopupField()
        // DrawLabel()
        //      draw Stat Name labels based on card type.
        //      200w * 40h
        // DrawEditableFields()
        //      draw the float fields for input of weights.
        // DrawControlButtons()
        //      draw buttons 160w * 50h
        //      Save Button needs to set asset to dirty.
        //      View Grid button will open a read-only view of the weights grid.
        // DrawSettingsAssetField()
        /*attributeSettings =
   EditorGUILayout.ObjectField("Attribute Settings", attributeSettings, typeof(AttributeSettings),
   false) as
   AttributeSettings;*/
        
        
       
    }
}