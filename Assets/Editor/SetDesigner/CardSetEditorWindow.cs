using Editor.CardData;
using Editor.CardEditor;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.SetDesigner
{
    public class CardSetEditorWindow : EditorWindow, ICustomEditorWindow
    {
        public Rect MainAreaRect { get; set; }

        [MenuItem("Tools/Set Editor")]
        public static void Init()
        {
            EditorWindow window = GetWindow<CardSetEditorWindow>("Card Set Editor");
            window.position = new Rect(50f, 50f, 600f, 650f);
            window.Show();
        }

        private void OnGUI()
        {
            SetUpAreaRects();
            DrawMainArea();
        }

        private void SetUpAreaRects()
        {
            MainAreaRect= new Rect(5,5,position.width - 10,position.height - 25);
        }

        private void DrawMainArea()
        {
            DrawToolbar();
            DrawCardListArea();
        }

        private void DrawToolbar()
        {
            // Draw any toolbar UI elements here
        }

        private void DrawCardListArea()
        {
            GUILayout.BeginArea(MainAreaRect);

            // Get all CardDataSO assets
            string[] cardDataAssets = AssetDatabase.FindAssets("t:CardDataSO");

            foreach (string guid in cardDataAssets)
            {
                // Load the asset
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CardDataSO cardData = AssetDatabase.LoadAssetAtPath<CardDataSO>(path);

                GUILayout.BeginHorizontal();

                if (cardData != null)
                {
                    // Display the card data name
                    GUILayout.Label(cardData.name, GUILayout.Width(200));

                    // Button to edit the card
                    if (GUILayout.Button("Edit", GUILayout.Width(50)))
                    {
                        Debug.Log("Edit card: " + cardData.name);
                        CardEditorWindow instance = GetWindow<CardEditorWindow>();
                        instance.OpenCardInEditor(cardData);
                    }

                    // Button to add card to current set
                    if (GUILayout.Button("+", GUILayout.Width(50)))
                    {
                        Debug.Log("Add card: " + cardData.name);
                        // Handle add card logic
                    }

                    // Button to remove card from current set
                    if (GUILayout.Button("-", GUILayout.Width(70)))
                    {
                        Debug.Log("Remove card: " + cardData.name);
                        // Handle remove card logic
                    }
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }
    }
}