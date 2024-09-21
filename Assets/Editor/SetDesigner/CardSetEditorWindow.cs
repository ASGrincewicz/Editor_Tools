using System.Collections.Generic;
using Editor.CardData;
using Editor.CardEditor;
using Editor.Utilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editor.SetDesigner
{
    public class CardSetEditorWindow : EditorWindow, ICustomEditorWindow
    {
        public Rect MainAreaRect { get; set; }
        private bool IsInEditMode { get; set; } = true;
        private string[] _cardSetAssetGUIDs;
        private string[] _cardSetNames;
        private int _selectedCardSetIndex = 0;
        private CardSetData _selectedCardSet;
        private string[] _cardAssetGUIDs;
        private HashSet<CardDataSO> _currentSet = new();
        private Vector2 _cardSetScrollPosition;

        [MenuItem("Tools/Set Editor")]
        public static void Init()
        {
            EditorWindow window = GetWindow<CardSetEditorWindow>("Card Set Editor");
            window.position = new Rect(50f, 50f, 600f, 500f);
            window.Show();
        }

        private void OnGUI()
        {
            SetUpAreaRects();
            DrawMainArea();
        }

        private void SetUpAreaRects()
        {
            MainAreaRect = new Rect(5, 100, position.width - 10, position.height);
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
           GetCardSetAssetsFromGUID();
           
           PopulateCardSetSelectionDropdownMenu();
            // Display the dropdown list
            _selectedCardSetIndex = EditorGUILayout.Popup("Select Card Set", _selectedCardSetIndex, _cardSetNames);

            // Handle card set selection logic
            if (GUILayout.Button("Select"))
            {
              LoadSelectedCardSet();
            }
        }

        private void GetCardSetAssetsFromGUID()
        {
            _cardSetAssetGUIDs = AssetDatabase.FindAssets("t:CardSetData");
        }

        private void PopulateCardSetSelectionDropdownMenu()
        {
            _cardSetNames = new string[_cardSetAssetGUIDs.Length];

            for (int i = 0; i < _cardSetAssetGUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(_cardSetAssetGUIDs[i]);
                CardSetData cardSet = AssetDatabase.LoadAssetAtPath<CardSetData>(path);
                _cardSetNames[i] = cardSet != null ? cardSet.name : "Unknown Asset";
            }

        }

        private void LoadSelectedCardSet()
        {
            string selectedPath = AssetDatabase.GUIDToAssetPath(_cardSetAssetGUIDs[_selectedCardSetIndex]);
            _selectedCardSet = AssetDatabase.LoadAssetAtPath<CardSetData>(selectedPath);
        }

        private void DrawCardListArea()
        {
            GUILayout.BeginArea(MainAreaRect);
           
            _cardSetScrollPosition = GUILayout.BeginScrollView(_cardSetScrollPosition, GUILayout.Height(MainAreaRect.height * 0.8f));
            GUILayout.BeginVertical(GUILayout.Height(MainAreaRect.height), GUILayout.ExpandHeight(true));
            DrawTotalCardsLabel();
           _cardAssetGUIDs = CardDataAssetUtility.CardAssetGUIDs;

            foreach (string guid in _cardAssetGUIDs)
            {
                CardDataSO cardData = CardDataAssetUtility.LoadCardDataByGuid(guid);

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
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawTotalCardsLabel()
        {
            string labelText = "";
            if (_selectedCardSet != null)
            {
                labelText = $"Total Cards: {_selectedCardSet.CardsInSet.Count} of {_selectedCardSet.NumberOfCards}\n";
               
            }
            else
            {
                labelText = "No Set Selected\n";
            }
            GUILayout.Label(labelText, EditorStyles.boldLabel);
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