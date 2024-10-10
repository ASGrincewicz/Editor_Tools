using System.Collections.Generic;
using Editor.CardData;
using Editor.Channels;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.SetDesigner
{
    public class CardSetEditorWindow : EditorWindow, ICustomEditorWindow
    {
        [SerializeField] private EditorWindowChannel _editorWindowChannel;
        private const string AssetPath = "Assets/Data/Scriptable Objects/Card Set Data/";
        private const string AssetFilter = "t:CardSetData";
        public Rect MainAreaRect { get; set; }
        private bool IsInEditMode { get; set; } = true;
        private string[] _cardSetAssetGUIDs;
        private string[] _cardSetNames;
        private int _selectedCardSetIndex = 0;
        private CardSetData _selectedCardSet;
        private string[] _cardAssetGUIDs;
        private HashSet<CardDataSO> _currentSet = new();
        private Vector2 _cardSetScrollPosition;
        
        // Editable Field Variables
        private CardSetData _newCardSet;
        private string _cardSetName;
        private CardSetType _cardSetType;
        private int _numberOfCards;
        private float _commonPercentage;
        private float _uncommonPercentage;
        private float _rarePercentage;
        private float _hyperRarePercentage;

        [MenuItem("Tools/Set Editor")]
        public static void Init()
        {
            EditorWindow window = GetWindow<CardSetEditorWindow>("Card Set Editor");
            window.position = new Rect(50f, 50f, 600f, 500f);
            window.Show();
        }

        private void OnEnable()
        {
            _editorWindowChannel.OnCardSetEditorWindowRequested += OpenCardSetEditorWindow;
        }

        private void OnDisable()
        {
            _editorWindowChannel.OnCardSetEditorWindowRequested -= OpenCardSetEditorWindow;
        }

        private void OpenCardSetEditorWindow()
        {
            Init();
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
           
            if (IsInEditMode)
            {
                DrawSetSelectionArea();
                DrawCardListArea();
            }
            else
            {
                DrawEditableFields();
            }
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
           DrawNewSetButton();
           
            if (IsInEditMode)
            {
                DrawSaveButton();
            }
            else
            {
               DrawSaveNewButton();
            }
           
            DrawEditButton();

            GUILayout.EndHorizontal();
        }

        private void DrawNewSetButton()
        {
            if (GUILayout.Button("New Set", EditorStyles.toolbarButton))
            {
                IsInEditMode = false;
                _selectedCardSet = null;
               //Debug.Log("New Set");
            }
        }

        private void DrawSaveButton()
        {
            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                //Debug.Log("Save");
                SaveCurrentSet();
            }
        }

        private void DrawSaveNewButton()
        {
            if (GUILayout.Button("Save New", EditorStyles.toolbarButton))
            {
                CreateNewCardSetAsset();
            }
        }

        private void DrawEditButton()
        {
            if (GUILayout.Button("Edit", EditorStyles.toolbarButton))
            {
                IsInEditMode = true;
                //Debug.Log("Edit");
            }
        }

        private void DrawEditableFields()
        {
            GUILayout.BeginVertical();
            _cardSetName = EditorGUILayout.TextField("CardSet Name", _cardSetName);
            _cardSetType = (CardSetType)EditorGUILayout.EnumPopup("CardSet Type", _cardSetType);
            _numberOfCards = EditorGUILayout.IntField("Number of Cards", _numberOfCards);
            _commonPercentage = EditorGUILayout.Slider("Common Percentage", _commonPercentage, 0, 1);
            _uncommonPercentage = EditorGUILayout.Slider("Uncommon Percentage", _uncommonPercentage, 0, 1);
            _rarePercentage = EditorGUILayout.Slider("Rare Percentage", _rarePercentage, 0, 1);
            _hyperRarePercentage = EditorGUILayout.Slider("Hyper Rare Percentage", _hyperRarePercentage, 0, 1);
            GUILayout.EndVertical();
        }

        private void CreateNewCardSetAsset()
        {
            //Debug.Log("Create new card set asset");
            _newCardSet = ScriptableObject.CreateInstance<CardSetData>();
            _newCardSet.CardSetName = _cardSetName;
            _newCardSet.CardSetType = _cardSetType;
            _newCardSet.NumberOfCards = _numberOfCards;
            _newCardSet.CommonPercentage = _commonPercentage;
            _newCardSet.UncommonPercentage = _uncommonPercentage;
            _newCardSet.RarePercentage = _rarePercentage;
            _newCardSet.HyperRarePercentage = _hyperRarePercentage;
            AssetDatabase.CreateAsset(_newCardSet, $"{AssetPath}{_cardSetName}.asset");
            EditorUtility.SetDirty(_newCardSet);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
            
            _selectedCardSetIndex = EditorGUILayout.Popup("Select Card Set", _selectedCardSetIndex, _cardSetNames);
            
            if (GUILayout.Button("Select"))
            {
              LoadSelectedCardSet();
            }
        }

        private void GetCardSetAssetsFromGUID()
        {
            _cardSetAssetGUIDs = AssetDatabase.FindAssets(AssetFilter);
        }

        private void PopulateCardSetSelectionDropdownMenu()
        {
            _cardSetNames = new string[_cardSetAssetGUIDs.Length];

            for (int i = 0; i < _cardSetAssetGUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(_cardSetAssetGUIDs[i]);
                CardSetData cardSet = AssetDatabase.LoadAssetAtPath<CardSetData>(path);
                _cardSetNames[i] = ReferenceEquals(cardSet, null) ? "Unknown Asset" : cardSet.name;
            }
        }

        private void LoadSelectedCardSet()
        {
            string selectedPath = AssetDatabase.GUIDToAssetPath(_cardSetAssetGUIDs[_selectedCardSetIndex]);
            _selectedCardSet = AssetDatabase.LoadAssetAtPath<CardSetData>(selectedPath);
            EditorGUIUtility.PingObject(_selectedCardSet);
            Selection.activeObject = _selectedCardSet;
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
                    
                    GUIStyle labelStyle = isInCurrentSet
                        ? new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, normal = new GUIStyleState { textColor = _selectedCardSet.setLabelColor } }
                        : GUI.skin.label;
                    GUILayout.Label(cardData.name, labelStyle, GUILayout.Width(200));
                    
                    if (GUILayout.Button("View", GUILayout.Width(50)))
                    {
                       // Debug.Log("View card: " + cardData.name);
                        EditorGUIUtility.PingObject(cardData);
                        Selection.activeObject = cardData;
                    }
                    
                    if (GUILayout.Button("Edit", GUILayout.Width(50)))
                    {
                       // Debug.Log("Edit card: " + cardData.name);
                       _editorWindowChannel.RaiseCardEditorWindowRequestedEvent(cardData);
                    }
                    
                    EditorGUI.BeginDisabledGroup(isInCurrentSet || _selectedCardSet == null);
                    if (GUILayout.Button("+", GUILayout.Width(50)))
                    {
                        if (_selectedCardSet != null)
                        {
                            AddCardToSet(cardData);
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                    
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

        // TODO: Turn into event
        private void AddCardToSet(CardDataSO cardData)
        {
            //Debug.Log("Add card: " + cardData.name);
            _selectedCardSet.AddCardToSet(cardData);
        }
        
        // TODO: Turn into event
        private void RemoveCardFromSet(CardDataSO cardData)
        {
           // Debug.Log("Remove card: " + cardData.name);
            _selectedCardSet.RemoveCardFromSet(cardData);
        }
    }
}