using System.Collections.Generic;
using System.Text;
using Editor.CardData;
using Editor.CardData.CardTypes;
using Editor.CardData.Stats;
using Editor.Channels;
using Editor.KeywordSystem;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.CardEditor
{
    public class CardEditorWindow : EditorWindow
    {
        [SerializeField] private EditorWindowChannel _editorWindowChannel;
        
        private static  EditorWindow _cardEditorWindow;
        // Constants
        private const float FIELD_WIDTH = 400;
        private const string ASSET_PATH = "Assets/Resources/Scriptable Objects/Card Types/";
        
        // GUI variables
        private Vector2 ScrollPosition { get; set; }
        private Rect MainAreaRect { get; set; }
        
        private string[] _cardTypeNames;
        private int _selectedCardTypeIndex;
        private List<CardStat> TempStats = new ();
        private CardTypeDataSO[] _allCardTypes;
        private CardTypeDataSO _loadedCardTypeData;
        
        private bool _typeLoaded = false;
        private bool _isCardLoaded = false;
        
        [MenuItem("Tools/Card Editor")]
        public static void Init()
        {
            _cardEditorWindow = GetWindow<CardEditorWindow>("Card Editor");
            _cardEditorWindow.position = new Rect(250f, 150f, 600f, 650f);
            _cardEditorWindow.Show();
            CardDataAssetUtility.CardTextStringBuilder = new StringBuilder();
            CardDataAssetUtility.LoadKeywordManagerAsset();
            CardDataAssetUtility.RefreshKeywordsList();
        }

        private void OnEnable()
        {
            _editorWindowChannel.OnCardEditorWindowRequested += OpenCardInEditor;
            LoadCardTypes();
        }

        private void OnDisable()
        {
            _editorWindowChannel.OnCardEditorWindowRequested -= OpenCardInEditor;
            CardDataAssetUtility.UnloadCard();
        }

        private void LoadCardTypes()
        {
            _allCardTypes = Resources.LoadAll<CardTypeDataSO>("");
            _cardTypeNames = new string[_allCardTypes.Length];
            for (int i = 0; i < _allCardTypes.Length; i++)
            {
                _cardTypeNames[i] = _allCardTypes[i].CardTypeName;
            }
            FindCardTypeAndSetIndex();
        }
        
        private void FindCardTypeAndSetIndex()
        {
            if (_loadedCardTypeData!= null)
            {
                _selectedCardTypeIndex = System.Array.IndexOf(_cardTypeNames, _loadedCardTypeData.CardTypeName);
            }
        }
        
        private void OpenCardInEditor(CardDataSO card)
        {
            Init();
            CardDataAssetUtility.CardToEdit = card;
            LoadCardData();
            _isCardLoaded = true;
        }
        private void OnGUI()
        {
            SetupAreaRects();
            DrawMainArea();
        }

        private void SetupAreaRects()
        {
            MainAreaRect= new Rect(5,5,position.width - 10,position.height - 25);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(MainAreaRect);
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawControlButtons();
            GUILayout.EndHorizontal();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawEditableFields();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawEditableFields()
        {
            EditorGUIUtility.labelWidth = 100;
            CardDataAssetUtility.CardToEdit = (CardDataSO)EditorGUILayout.ObjectField(
                "Card To Edit", CardDataAssetUtility.CardToEdit, typeof(CardDataSO), false);
            if (CardDataAssetUtility.CardToEdit != null && !_isCardLoaded)
            {
                LoadCardData();
                _isCardLoaded = true;
            }
            
            if (_cardTypeNames == null || _cardTypeNames.Length == 0)
            {
                Debug.LogError("Card type names array is not initialized or empty.");
                return;
            }
            
            _selectedCardTypeIndex = EditorGUILayout.Popup("Card Type", _selectedCardTypeIndex, _cardTypeNames);
            
            if (_selectedCardTypeIndex >= 0 && _selectedCardTypeIndex < _cardTypeNames.Length)
            {
                _loadedCardTypeData = _allCardTypes[_selectedCardTypeIndex];
                if (GUILayout.Button("Change Card Type", EditorStyles.toolbarButton))
                {
                    CardDataAssetUtility.CardTypeData = _allCardTypes[_selectedCardTypeIndex];
                }
                if (CardDataAssetUtility.CardTypeData == null)
                {
                    CardDataAssetUtility.LoadCardTypeData(_loadedCardTypeData);
                }

                if (_loadedCardTypeData == null)
                {
                    Debug.LogError($"Failed to load CardTypeDataSO named {_cardTypeNames[_selectedCardTypeIndex]}");
                }
            }
            else
            {
                Debug.LogWarning("Invalid selection index.");
            }

            CardDataAssetUtility.CardName = EditorGUILayout.TextField("Card Name", 
                CardDataAssetUtility.CardName, GUILayout.Width(FIELD_WIDTH));

            CardDataAssetUtility.CardRarity = (CardRarity)EditorGUILayout.EnumPopup("Card Rarity",
                CardDataAssetUtility.CardRarity, GUILayout.Width(FIELD_WIDTH));

            CardDataAssetUtility.Artwork = (Texture2D)EditorGUILayout.ObjectField("Artwork",
                CardDataAssetUtility.Artwork, typeof(Texture2D), false,
                GUILayout.Height(200), GUILayout.Width(FIELD_WIDTH));

            DrawFieldsBasedOnType();
        }

        private void DrawFieldsBasedOnType()
        {
            if (_loadedCardTypeData != null)
            {
                if (!_typeLoaded)
                {
                    CardDataAssetUtility.LoadCardTypeData(_loadedCardTypeData);
                    _typeLoaded = true;
                }
                if (_loadedCardTypeData.HasStats)
                {
                    CardDataAssetUtility.CardToEdit ??= CreateInstance<CardDataSO>();
                    TempStats = CardDataAssetUtility.CardStats;
                    int totalStats = TempStats.Count;

                    for (int i = 0; i < totalStats; i++)
                    {
                        DrawStatLayout(TempStats[i].statName, ref TempStats[i].statValue, TempStats[i].statDescription);
                    }
                }

                if (_loadedCardTypeData.HasKeywords)
                {
                    DrawKeywordArea();
                }

                if (_loadedCardTypeData.HasCost)
                {
                    GUILayout.Label($"Card Cost: {CardDataAssetUtility.CardCost}");
                }

                if (_loadedCardTypeData.HasCardText)
                {
                    GUILayout.Label("Card Text");
                    CardDataAssetUtility.CardText = EditorGUILayout.TextArea(CardDataAssetUtility.CardText,
                        GUILayout.Height(100), GUILayout.Width(FIELD_WIDTH));
                }
            }
        }
        
        private void DrawStatLayout(string statName, ref int statValue, string statDescription)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH), GUILayout.ExpandWidth(true));
            DrawStatName(statName);
            DrawStatValueField(ref statValue);
            DrawStatDescription(statDescription);
            GUILayout.EndHorizontal();
        }

        private void DrawStatName(string statName)
        {
            GUILayout.BeginVertical(GUILayout.Width(100));
            GUILayout.Label(statName);
            GUILayout.EndVertical();
        }

        private void DrawStatDescription(string statDescription)
        {
            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.Label(statDescription);
            GUILayout.EndVertical();
        }

        private void DrawStatValueField(ref int statValue)
        {
            GUILayout.BeginVertical(GUILayout.Width(100));
            statValue = EditorGUILayout.IntField(statValue);
            GUILayout.EndVertical();
        }

        private void DrawKeywordArea()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH));
            EditorGUIUtility.labelWidth = 100;
            GUILayout.Label("Keywords");
            if (CardDataAssetUtility.SelectedKeywords is not { Length: 3 })
            {
                CardDataAssetUtility.SelectedKeywords = new Keyword[3];
                CardDataAssetUtility.SelectedKeywordsIndex = new int[3]; // Initialize the index array only once.
            }

            for (int i = 0; i < CardDataAssetUtility.SelectedKeywords.Length; i++)
            {
               CheckAndResetSelectedKeywordIndex(i);

                DrawKeywordFields(i);
                
                if (CardDataAssetUtility.SelectedKeywordsIndex[i] < 0 || CardDataAssetUtility.SelectedKeywordsIndex[i] >= CardDataAssetUtility.KeywordNamesList.Count)
                {
                    Debug.LogError($"Index {CardDataAssetUtility.SelectedKeywordsIndex[i]} is out of range for _keywordNamesList with count {CardDataAssetUtility.KeywordNamesList.Count}");
                    continue;
                }
                
                SetKeyword(i);
            }
            GUILayout.EndHorizontal();
        }

        private void CheckAndResetSelectedKeywordIndex(int index)
        {
            if (CardDataAssetUtility.SelectedKeywordsIndex.Length <= index)
            {
                Debug.LogError($"Index {index} is out of range for _selectedKeywordsIndex");
            }
                
            if (CardDataAssetUtility.SelectedKeywordsIndex[index] == -1)
            {
                CardDataAssetUtility.SelectedKeywordsIndex[index] = 0;
            }
        }

        private void DrawKeywordFields(int index)
        {
            if (CardDataAssetUtility.SelectedKeywordsIndex != null && CardDataAssetUtility.KeywordNamesList != null && CardDataAssetUtility.KeywordNamesList.Count > 0)
            {
                CardDataAssetUtility.SelectedKeywordsIndex[index] = EditorGUILayout.Popup(
                    CardDataAssetUtility.SelectedKeywordsIndex[index],
                    CardDataAssetUtility.KeywordNamesList.ToArray(),
                    GUILayout.Width(FIELD_WIDTH / 3)
                );
            }
            else
            {
                CardDataAssetUtility.SelectedKeywordsIndex[index] = 0;
                EditorGUILayout.Popup(
                    0,
                    new string[] { "No Keywords Available" },
                    GUILayout.Width(FIELD_WIDTH / 3)
                );
            }
        }

        private void SetKeyword(int index)
        {
            if (CardDataAssetUtility.KeywordNamesList.Count > 0)
            {
                string selectedKeywordName = CardDataAssetUtility.KeywordNamesList[CardDataAssetUtility.SelectedKeywordsIndex[index]];
                CardDataAssetUtility.SelectedKeywords[index] = CardDataAssetUtility.KeywordManager.GetKeywordByName(selectedKeywordName);
            }
        }

        private void DrawControlButtons()
        {
            if (GUILayout.Button("Create", EditorStyles.toolbarButton))
            {
                HandleCreateButtonPressed();
            }
            if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            {
               HandleLoadButtonPressed();
            }
            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                HandleSaveButtonPressed();
            }
            if(GUILayout.Button("Unload", EditorStyles.toolbarButton) && !ReferenceEquals(CardDataAssetUtility.CardToEdit, null))
            {
               HandleUnloadButtonPressed();
            }

            if (GUILayout.Button("Calculate Cost", EditorStyles.toolbarButton) && !ReferenceEquals(CardDataAssetUtility.CardToEdit, null))
            {
               HandleCalculateCostButtonPressed();
            }

            if (GUILayout.Button("Done", EditorStyles.toolbarButton))
            {
               HandleDoneButtonPressed();
            }
        }

        private void HandleCreateButtonPressed()
        {
            CardDataAssetUtility.CreateNewCard(TempStats);
        }

        private void HandleSaveButtonPressed()
        {
            CardDataAssetUtility.SaveExistingCard(TempStats);
        }

        private void HandleLoadButtonPressed()
        {
            LoadCardData();
        }

        private void HandleUnloadButtonPressed()
        {
            CardDataAssetUtility.UnloadCard();
            _isCardLoaded = false;
        }

        private void HandleCalculateCostButtonPressed()
        {
            _editorWindowChannel.RaiseCostCalculatorWindowRequestedEvent(CardDataAssetUtility.CardToEdit);
        }

        private void HandleDoneButtonPressed()
        {
            _cardEditorWindow.Close();
        }

        private void LoadCardData()
        {
            LoadCardTypeData();
            FindCardTypeAndSetIndex();
            TempStats = CardDataAssetUtility.CardStats;
        }

        private void LoadCardTypeData()
        {
            CardDataAssetUtility.CardToEdit.CardTypeDataSO = _loadedCardTypeData;
            CardDataAssetUtility.LoadCardFromFile();
            _loadedCardTypeData = CardDataAssetUtility.CardTypeData;
        }
    }
}