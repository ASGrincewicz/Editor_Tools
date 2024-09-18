using System.Collections.Generic;
using Editor.CardData;
using Editor.CardEditor;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editor.SetDesigner
{
    public class CardSetEditorWindow : EditorWindow, ICustomEditorWindow
    {
        public Rect MainAreaRect { get; set; }
        private bool IsInEditMode { get; set; } = true;
        private string[] _cardSetNames;
        private int _selectedCardSetIndex = 0;
        private CardSetData _selectedCardSet;
        private HashSet<CardDataSO> _currentSet = new();

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
            MainAreaRect = new Rect(5, 100, position.width - 10, position.height - 25);
        }

        private void DrawMainArea()
        {
            DrawToolbar();
            DrawSetSelectionArea();
            if (IsInEditMode)
            {
                DrawCardListArea();
            }
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("New Set", EditorStyles.toolbarButton))
            {
                Debug.Log("New Set");
            }

            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                Debug.Log("Save");
                SaveCurrentSet();
            }

            if (GUILayout.Button("Edit", EditorStyles.toolbarButton))
            {
                IsInEditMode = !IsInEditMode;
                Debug.Log("Edit");
            }

            GUILayout.EndHorizontal();
        }

        private void SaveCurrentSet()
        {
            EditorUtility.SetDirty(_selectedCardSet);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void DrawSetSelectionArea()
        {
            // Find existing CardSetData assets
            string[] cardSetAssets = AssetDatabase.FindAssets("t:CardSetData");

            // Initialize or update the array of asset names
            _cardSetNames = new string[cardSetAssets.Length];

            for (int i = 0; i < cardSetAssets.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(cardSetAssets[i]);
                CardSetData cardSet = AssetDatabase.LoadAssetAtPath<CardSetData>(path);
                _cardSetNames[i] = cardSet != null ? cardSet.name : "Unknown Asset";
            }

            // Display the dropdown list
            _selectedCardSetIndex = EditorGUILayout.Popup("Select Card Set", _selectedCardSetIndex, _cardSetNames);

            // Handle card set selection logic
            if (GUILayout.Button("Select"))
            {
                string selectedPath = AssetDatabase.GUIDToAssetPath(cardSetAssets[_selectedCardSetIndex]);
                _selectedCardSet = AssetDatabase.LoadAssetAtPath<CardSetData>(selectedPath);

                if (_selectedCardSet != null)
                {
                    Debug.Log("Selected Card Set: " + _selectedCardSet.name);
                    // Implement additional logic for when a card set is selected
                }
            }
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
                    bool isInCurrentSet = _selectedCardSet != null && _selectedCardSet.CardsInSet != null && _selectedCardSet.CardsInSet.Contains(cardData);

                    // Display the card data name with bold font if it is in the current set
                    GUIStyle labelStyle = isInCurrentSet
                        ? new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, normal = new GUIStyleState { textColor = _selectedCardSet.isSetLabelColor } }
                        : GUI.skin.label;
                    GUILayout.Label(cardData.name, labelStyle, GUILayout.Width(200));
                    
                    if (GUILayout.Button("View", GUILayout.Width(50)))
                    {
                        Debug.Log("View card: " + cardData.name);
                        EditorGUIUtility.PingObject(cardData);
                        Selection.activeObject = cardData;
                    }

                    // Button to edit the card
                    if (GUILayout.Button("Edit", GUILayout.Width(50)))
                    {
                        Debug.Log("Edit card: " + cardData.name);
                        CardEditorWindow instance = GetWindow<CardEditorWindow>();
                        instance.OpenCardInEditor(cardData);
                    }

                    // Button to add card to current set
                    EditorGUI.BeginDisabledGroup(isInCurrentSet || _selectedCardSet == null);
                    if (GUILayout.Button("+", GUILayout.Width(50)))
                    {
                        if (_selectedCardSet != null)
                        {
                            AddCardToSet(cardData);
                        }
                    }
                    EditorGUI.EndDisabledGroup();

                    // Button to remove card from current set
                    EditorGUI.BeginDisabledGroup(!isInCurrentSet || _selectedCardSet == null);
                    if (GUILayout.Button("-", GUILayout.Width(50)))
                    {
                        if (_selectedCardSet != null)
                        {
                            RemoveCardFromSet(cardData);
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }


        private void AddCardToSet(CardDataSO cardData)
        {
            Debug.Log("Add card: " + cardData.name);
            _selectedCardSet.AddCardToSet(cardData);
        }

        private void RemoveCardFromSet(CardDataSO cardData)
        {
            Debug.Log("Remove card: " + cardData.name);
            _selectedCardSet.RemoveCardFromSet(cardData);
        }
    }
}