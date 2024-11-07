using System.Collections.Generic;
using System.Text;
using Editor.CardData;
using Editor.CardData.CardTypeData;
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
        
        // GUI variables
        private Vector2 ScrollPosition { get; set; }
        private Rect MainAreaRect { get; set; }
        
        // Main Area Buttons Setup
        private bool IsCreateCardButtonPressed => GUILayout.Button("Create", EditorStyles.toolbarButton);

        private bool IsLoadCardButtonPressed => GUILayout.Button("Load Card", EditorStyles.toolbarButton);

        private bool IsSaveCardButtonPressed => GUILayout.Button("Save Card", EditorStyles.toolbarButton);

        private bool IsUnloadCardButtonPressed => GUILayout.Button("Unload Card", EditorStyles.toolbarButton);

        private bool IsCalculateCostButtonPressed => GUILayout.Button("Calculate Cost", EditorStyles.toolbarButton);
        
        private bool IsDoneButtonPressed => GUILayout.Button("Done", EditorStyles.toolbarButton);
        private List<CardStat> TempStats = new List<CardStat>();
        
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
        }

        private void OnDisable()
        {
            _editorWindowChannel.OnCardEditorWindowRequested -= OpenCardInEditor;
            CardDataAssetUtility.SelectedCard = null;
        }
        
        private void OpenCardInEditor(CardDataSO card)
        {
            Init();
            CardDataAssetUtility.CardToEdit = card;
            CardDataAssetUtility.LoadCardFromFile();
            TempStats = CardDataAssetUtility.CardStats;
            //CardDataAssetUtility.StatsLoaded = false;
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
            CardDataAssetUtility.CardToEdit =
                EditorGUILayout.ObjectField("Card To Edit", CardDataAssetUtility.CardToEdit, typeof(CardDataSO), false)
                    as CardDataSO;
            GUILayout.Label(
                !ReferenceEquals(CardDataAssetUtility.SelectedCard, null) ? "Select Card Type" : "Create New Card",
                EditorStyles.boldLabel);
            CardDataAssetUtility.CardTypeData = (CardTypeDataSO)EditorGUILayout.ObjectField("Card Type",
                CardDataAssetUtility.CardTypeData, typeof(CardTypeDataSO), false);
            if (GUILayout.Button("Change Card Type"))
            {
                CardDataAssetUtility.LoadCardTypeData();
                TempStats = CardDataAssetUtility.CardStats;
            }

            CardDataAssetUtility.CardName = EditorGUILayout.TextField("Card Name", CardDataAssetUtility.CardName,
                GUILayout.Width(FIELD_WIDTH));
            CardDataAssetUtility.CardRarity = (CardRarity)EditorGUILayout.EnumPopup("Card Rarity",
                CardDataAssetUtility.CardRarity, GUILayout.Width(FIELD_WIDTH));
            CardDataAssetUtility.Artwork = (Texture2D)EditorGUILayout.ObjectField("Artwork",
                CardDataAssetUtility.Artwork, typeof(Texture2D), false,
                GUILayout.Height(200), GUILayout.Width(FIELD_WIDTH));
            if (CardDataAssetUtility.CardTypeData != null)
            {
                if (CardDataAssetUtility.CardTypeData.HasStats)
                {
                    CardDataAssetUtility.CardToEdit ??= CreateInstance<CardDataSO>();
                    if (!CardDataAssetUtility.StatsLoaded)
                    {
                        CardDataAssetUtility.LoadCardTypeData();
                        TempStats = CardDataAssetUtility.CardStats;
                    }
                    int totalStats = CardDataAssetUtility.CardStats.Count;
                    
                    
                    for (int i = 0; i < totalStats; i++)
                    {
                        CardStat stat = TempStats[i];
                        int statValue = stat.StatValue;
                        DrawStatLayout(stat.StatName, ref statValue, stat.StatDescription);
                    }
                }
                if (CardDataAssetUtility.CardTypeData.HasKeywords)
                {
                    DrawKeywordArea();
                }

                if (CardDataAssetUtility.CardTypeData.HasCost)
                {
                    GUILayout.Label($"Card Cost: {CardDataAssetUtility.CardCost}");
                        
                }

                if (CardDataAssetUtility.CardTypeData.HasCardText)
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
                if (CardDataAssetUtility.SelectedKeywordsIndex.Length <= i)
                {
                    Debug.LogError($"Index {i} is out of range for _selectedKeywordsIndex");
                    continue;
                }
                
                if (CardDataAssetUtility.SelectedKeywordsIndex[i] == -1)
                {
                    CardDataAssetUtility.SelectedKeywordsIndex[i] = 0;
                }

                if (CardDataAssetUtility.SelectedKeywordsIndex != null && CardDataAssetUtility.KeywordNamesList != null && CardDataAssetUtility.KeywordNamesList.Count > 0)
                {
                    CardDataAssetUtility.SelectedKeywordsIndex[i] = EditorGUILayout.Popup(
                        CardDataAssetUtility.SelectedKeywordsIndex[i],
                        CardDataAssetUtility.KeywordNamesList.ToArray(),
                        GUILayout.Width(FIELD_WIDTH / 3)
                    );
                }
                else
                {
                    CardDataAssetUtility.SelectedKeywordsIndex[i] = 0;
                    EditorGUILayout.Popup(
                        0,
                        new string[] { "No Keywords Available" },
                        GUILayout.Width(FIELD_WIDTH / 3)
                    );
                }
                
                if (CardDataAssetUtility.SelectedKeywordsIndex[i] < 0 || CardDataAssetUtility.SelectedKeywordsIndex[i] >= CardDataAssetUtility.KeywordNamesList.Count)
                {
                    Debug.LogError($"Index {CardDataAssetUtility.SelectedKeywordsIndex[i]} is out of range for _keywordNamesList with count {CardDataAssetUtility.KeywordNamesList.Count}");
                    continue;
                }
                
                if (CardDataAssetUtility.KeywordNamesList.Count > 0)
                {
                    string selectedKeywordName = CardDataAssetUtility.KeywordNamesList[CardDataAssetUtility.SelectedKeywordsIndex[i]];
                    CardDataAssetUtility.SelectedKeywords[i] = CardDataAssetUtility.keywordManager.GetKeywordByName(selectedKeywordName);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawControlButtons()
        {
            if (IsCreateCardButtonPressed)
            {
                CardDataAssetUtility.CreateNewCard();
            }
            if (IsLoadCardButtonPressed)
            {
                CardDataAssetUtility.LoadCardFromFile();
                TempStats = CardDataAssetUtility.CardStats;
                Debug.Log($"Temp stats loaded:{TempStats.Count}");
            }
            if (IsSaveCardButtonPressed)
            {
                CardDataAssetUtility.SaveExistingCard();
            }
            if(IsUnloadCardButtonPressed && !ReferenceEquals(CardDataAssetUtility.SelectedCard, null))
            {
                CardDataAssetUtility.UnloadCard();
            }

            if (IsCalculateCostButtonPressed && !ReferenceEquals(CardDataAssetUtility.SelectedCard, null))
            {
               _editorWindowChannel.RaiseCostCalculatorWindowRequestedEvent(CardDataAssetUtility.SelectedCard);
            }

            if (IsDoneButtonPressed)
            {
               _cardEditorWindow.Close();
            }
        }
    }
}