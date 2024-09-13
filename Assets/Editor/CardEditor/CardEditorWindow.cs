using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.AttributesWeights;
using Editor.CardData;
using Editor.CostCalculator;
using Editor.KeywordSystem;
using Editor.Utilities;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using static Editor.CardData.StatDataReference;


namespace Editor.CardEditor
{
    public class CardEditorWindow : EditorWindow
    {
        [SerializeField] private KeywordManager _keywordManager;

        [SerializeField] private Texture2D _placeholderArtwork;
        // Constants
        private const string ASSET_PATH = "Assets/Data/Scriptable Objects/Cards/";

        private const string ALLY_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Ally_Weights.asset";

        private const string BOSS_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Boss_Weights.asset";

        private const string CREATURE_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Creature_Weights.asset";
        private const string ENVIRONMENT_WEIGHT_ASSET_PATH = "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Environment_Weights.asset";
        private const string GEAR_WEIGHT_ASSET_PATH = "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Gear_Weights.asset";
        private const string HUNTER_WEIGHT_ASSET_PATH = "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Hunter_Weights.asset";

        private const string KEYWORD_ONLY_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Keyword-Only_Weights.asset";
        private const string ASSET_FILTER = "t:CardSO";
        private const float FIELD_WIDTH = 400;
        
        // Class variables
        private List<CardSO> AllCards { get; } = new();
        private Dictionary<CardSO, bool> SelectedCards { get; } = new();
        private CardSO CardToEdit { get; set; }
        
        // Method specific variables
        private StringBuilder CardTextStringBuilder { get; set; }
        private CardTypes CardTypes { get; set; }
        private CardRarity CardRarity { get; set; }
        private string CardName { get; set; }
        [CanBeNull] private Texture2D Artwork { get; set; }

        private CardStat? AttackStat { get; set; }
        private int _attackValue;
        private CardStat? ExploreStat { get; set; }
        private int _exploreValue;
        private CardStat? FocusStat { get; set; }
        private int _focusValue;
        private CardStat? HitPointsStat{get; set; }
        private int _hitPointsValue;
        private CardStat? SpeedStat{get; set; }
        private int _speedValue;
        private CardStat? UpgradeSlotsStat { get; set; }
        private int _upgradeSlotsValue;

        private List<Keyword> KeywordsList { get; set; }
        private List<string> KeywordNamesList { get; set; }
        private Keyword[] SelectedKeywords { get; set; }
        private int[] SelectedKeywordsIndex { get; set; }
        private string CardText { get; set; }
        private int CardCost { get; set; }
        [CanBeNull] private CardSO SelectedCard { get; set; }
        
        
        // GUI variables
        private Vector2 ScrollPosition { get; set; }
        private Vector2 ScrollPosition2 { get; set; }
        private Rect MainAreaRect { get; set; }
        private Rect SecondAreaRect { get; set; }
        private int _cardListAreaColumns = 2;
        
        // Main Area Buttons Setup
        private bool IsCreateCardButtonPressed => GUILayout.Button("Create", EditorStyles.toolbarButton);

        private bool IsLoadCardButtonPressed => GUILayout.Button("Load Card", EditorStyles.toolbarButton);

        private bool IsSaveCardButtonPressed => GUILayout.Button("Save Card", EditorStyles.toolbarButton);

        private bool IsUnloadCardButtonPressed => GUILayout.Button("Unload Card", EditorStyles.toolbarButton);

        private bool IsCalculateCostButtonPressed => GUILayout.Button("Calculate Cost", EditorStyles.toolbarButton);

        // Card list area buttons setup
        private bool IsEditSelectedCardButtonPressed => GUILayout.Button("Edit Selected Card", EditorStyles.toolbarButton);

        private bool IsRefreshKeywordsButtonPressed => GUILayout.Button("Refresh Keywords", EditorStyles.toolbarButton);

        private bool IsRefreshCardListButtonPressed => GUILayout.Button("Refresh Card List", EditorStyles.toolbarButton);

        [MenuItem("Tools/Card Editor")]
        public static void Init()
        {
            EditorWindow window = GetWindow<CardEditorWindow>("Card Editor");
            window.position = new Rect(50f, 50f, 500f, 800f);
            window.Show();
        }
        public void OpenCardInEditor(CardSO card)
        {
            CardToEdit = card;
            LoadCardFromFile();
        }
        
        private void OnEnable()
        {
            string[] guids = AssetDatabase.FindAssets(ASSET_FILTER, new[] { ASSET_PATH });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                CardSO card =  ErrorHandler.TryToGetCard(AssetDatabase.LoadAssetAtPath<CardSO>(assetPath));

                if (ErrorHandler.VerifyCardNotInList(AllCards, card))
                {
                    AllCards.Add(card);
                }
                SelectedCards[card] = false;
            }
            RefreshKeywordsList();
            
            CardTextStringBuilder = new StringBuilder();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawMainArea();
            DrawCardListArea();
        }

        private void SetupAreaRects()
        {
            MainAreaRect= new Rect(5,5,position.width - 10,position.height - 225);
            SecondAreaRect = new Rect(5, MainAreaRect.y + MainAreaRect.height + 20, position.width - 10, position.height - MainAreaRect.height - 50);
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
        
        private void DrawControlButtons()
        {
            if (ReferenceEquals(SelectedCard, null) )
            {
                if (IsCreateCardButtonPressed)
                {
                    CreateNewCard();
                }
                if (IsLoadCardButtonPressed)
                {
                    LoadCardFromFile();
                }
            }
            else
            {
                if (IsSaveCardButtonPressed)
                {
                    SaveExistingCard();
                }
                if(IsUnloadCardButtonPressed && !ReferenceEquals(SelectedCard, null))
                {
                    UnloadCard();
                }

                if (IsCalculateCostButtonPressed && !ReferenceEquals(SelectedCard, null))
                {
                    CostCalculatorWindow instance = EditorWindow.GetWindow<CostCalculatorWindow>();
                    instance.OpenInCostCalculatorWindow(SelectedCard);
                }
            }
        }

        private void DrawEditableFields()
        {
            EditorGUIUtility.labelWidth = 100;
            CardToEdit = EditorGUILayout.ObjectField("Card To Edit",CardToEdit, typeof(CardSO),false) as CardSO;
            GUILayout.Label(!ReferenceEquals(SelectedCard, null) ? "Select Card Type" : "Create New Card", EditorStyles.boldLabel);
            DrawCommonFields();
            DrawKeywordArea();
        }

        private void DrawCommonFields()
        {
            CardTypes = (CardTypes)EditorGUILayout.EnumPopup("Card Type",CardTypes, GUILayout.Width(FIELD_WIDTH));
           
            CardName = EditorGUILayout.TextField("Card Name", CardName, GUILayout.Width(FIELD_WIDTH));
            CardRarity = (CardRarity)EditorGUILayout.EnumPopup("Card Rarity",CardRarity,GUILayout.Width(FIELD_WIDTH));
            Artwork = (Texture2D)EditorGUILayout.ObjectField("Artwork", Artwork, typeof(Texture2D), false,
                GUILayout.Height(200), GUILayout.Width(FIELD_WIDTH));
            DetermineStatLayout();
            GUILayout.Label($"Card Cost: {CardCost}"); ;
            GUILayout.Label("Card Text");
            CardText = EditorGUILayout.TextArea(CardText, GUILayout.Height(100), GUILayout.Width(FIELD_WIDTH));
        }

        private void DetermineStatLayout()
        {
            switch (CardTypes)
            {
                case CardTypes.TBD:
                case CardTypes.Starship:
                    break;
                case CardTypes.Action:
                    break;
                case CardTypes.Environment:
                    DrawStatLayout(StatNames.Explore, ref _exploreValue, EXPLORE_DESCRIPTION);
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                case CardTypes.Character_Ally:
                case CardTypes.Character_Hunter:
                case CardTypes.Creature:
                case CardTypes.Boss:
                    DrawStatLayout(StatNames.Attack, ref _attackValue, ATTACK_DESCRIPTION);
                    DrawStatLayout(StatNames.HP, ref _hitPointsValue, HIT_POINTS_DESCRIPTION);
                    DrawStatLayout(StatNames.Speed, ref _speedValue, SPEED_DESCRIPTION);
                    DrawStatLayout(StatNames.Focus, ref _focusValue, FOCUS_DESCRIPTION);
                    if (CardTypes == CardTypes.Character_Hunter) DrawStatLayout(StatNames.Upgrades, ref _upgradeSlotsValue, UPGRADE_SLOTS_DESCRIPTION);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void DrawStatLayout(StatNames statName, ref int statValue, string statDescription)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
            DrawStatName(statName);
            DrawStatValueField(ref statValue);
            DrawStatDescription(statDescription);
            GUILayout.EndHorizontal();
        }  
        
        private void DrawStatName(StatNames statName)
        {
            GUILayout.BeginVertical(GUILayout.Width(100));
            GUILayout.Label(statName.GetDescription());
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
            statValue = EditorGUILayout.IntField(statValue,GUILayout.Width(50),GUILayout.ExpandWidth(false));
            GUILayout.EndVertical();
        }

        private void DrawKeywordArea()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH));
            EditorGUIUtility.labelWidth = 100;
            GUILayout.Label("Keywords");
            if (SelectedKeywords is not { Length: 3 })
            {
                SelectedKeywords = new Keyword[3];
                SelectedKeywordsIndex = new int[3]; // Initialize the index array only once.
            }

            for (int i = 0; i < SelectedKeywords.Length; i++)
            {
                if (SelectedKeywordsIndex.Length <= i)
                {
                    Debug.LogError($"Index {i} is out of range for _selectedKeywordsIndex");
                    continue;
                }
                
                if (SelectedKeywordsIndex[i] == -1)
                {
                    SelectedKeywordsIndex[i] = 0;
                }

                SelectedKeywordsIndex[i] = EditorGUILayout.Popup(
                    SelectedKeywordsIndex[i],
                    KeywordNamesList.ToArray(),
                    GUILayout.Width(FIELD_WIDTH / 3)
                );
                
                if (SelectedKeywordsIndex[i] < 0 || SelectedKeywordsIndex[i] >= KeywordNamesList.Count)
                {
                    Debug.LogError($"Index {SelectedKeywordsIndex[i]} is out of range for _keywordNamesList with count {KeywordNamesList.Count}");
                    continue;
                }

                // Use Find method to assign Keyword to _selectedKeywords
                if (KeywordNamesList.Count > 0)
                {
                    string selectedKeywordName = KeywordNamesList[SelectedKeywordsIndex[i]];
                    SelectedKeywords[i] = _keywordManager.GetKeywordByName(selectedKeywordName);
                }
            }
            GUILayout.EndHorizontal();
        }
        

        private void DrawCardListArea()
        {
            GUILayout.BeginArea(SecondAreaRect);
            GUILayout.Label("Card List", EditorStyles.boldLabel);
            DrawCardListAreaButtons();
            ScrollPosition2 = GUILayout.BeginScrollView(ScrollPosition2, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
    
            int columnCounter = 0;
    
            foreach (CardSO card in AllCards)
            {
                if (columnCounter % _cardListAreaColumns == 0)
                {
                    GUILayout.BeginHorizontal();
                }
        
                DrawCardListingWithCheckbox(card);

                if (columnCounter % _cardListAreaColumns == _cardListAreaColumns - 1)
                {
                    GUILayout.EndHorizontal();
                }
        
                columnCounter++;
            }

            if (columnCounter % _cardListAreaColumns != 0)
            {
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
    
            GUILayout.EndArea();
        }

        private void DrawCardListingWithCheckbox(CardSO card)
        {
            GUILayout.BeginVertical(GUILayout.Width(SecondAreaRect.width / _cardListAreaColumns));
            SelectedCards[card] = EditorGUILayout.ToggleLeft($"{card.CardName}/{card.CardType}/{card.Rarity}", SelectedCards[card], GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
        }

        private void DrawCardListAreaButtons()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawEditSelectedCardButton();
            DrawRefreshKeywordsButton();
            DrawRefreshCardListButton();
           
            GUILayout.EndHorizontal();
        }

        private void DrawEditSelectedCardButton()
        {
            if (IsEditSelectedCardButtonPressed)
            {
                EditSelectedCard();
            }
        }

        private void DrawRefreshKeywordsButton()
        {
            if (IsRefreshKeywordsButtonPressed)
            {
                RefreshKeywordsList();
            }
        }

        private void DrawRefreshCardListButton()
        {
            if (IsRefreshCardListButtonPressed)
            {
                RefreshCardList();
            }
        }
        private void CreateNewCard()
        {
            CardSO newCard = CreateInstance<CardSO>();

            InitializeCard(newCard);
            AssetDatabase.CreateAsset(newCard, $"{ASSET_PATH}{CardName}.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newCard;
            RefreshCardList();
            CardToEdit = newCard;
           
        }
        
        private void EditSelectedCard()
        {
            KeyValuePair<CardSO, bool> cardToModify = new();
            foreach (KeyValuePair<CardSO, bool> entry in SelectedCards)
            {
                if (entry.Value)
                {
                    cardToModify = entry;
                }
            }
            
            SetSelectedCard(cardToModify.Key, cardToModify.Value);
            CardToEdit = cardToModify.Key;
            LoadCardFromFile();
            CardTextStringBuilder.Clear();
        }
        
        private void RefreshKeywordsList()
        {
            KeywordsList = new List<Keyword>();
            foreach (Keyword keyword in _keywordManager.keywordList)
            {
                if (!keyword.IsDefault())
                {
                    KeywordsList.Add(keyword);
                }
            }
            KeywordNamesList = new List<string>();
            foreach (Keyword keyword in KeywordsList.ToList())
            {
                if (!keyword.IsDefault())
                {
                    KeywordNamesList.Add(keyword.keywordName);
                }
            }
        }
        
        private void LoadCardFromFile()
        {
            UnloadCard();
            if (!ReferenceEquals(CardToEdit, null))
            {
                SetSelectedCard(CardToEdit, true);
            }
            else
            {
                string path = EditorUtility.OpenFilePanel("Select Card", ASSET_PATH, "asset");
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path[Application.dataPath.Length..];
                    SetSelectedCard(AssetDatabase.LoadAssetAtPath<CardSO>(path), true);
                    CardToEdit = SelectedCard;
                }
            }
            ErrorHandler.TryToGetCard(SelectedCard);
            LoadCommonCardData();
            UpdateSelectedKeywordsIndices();
            CardText += CardTextStringBuilder.Append(SelectedCard.CardText);
            GetStatsFromLoadedCard();
            CardTextStringBuilder.Clear();;
        }

        private void LoadCommonCardData()
        {
            CardTypes = SelectedCard.CardType;
            CardName = SelectedCard.CardName;
            CardRarity = SelectedCard.Rarity;
            Artwork = SelectedCard.ArtWork;
            CardCost = SelectedCard.CardCost;
        }
        
        private void UpdateSelectedKeywordsIndices()
        {
            if (SelectedCard?.Keywords == null)
            {
                Debug.LogError("Selected card or its keywords are null");
                return;
            }

            SelectedKeywords = SelectedCard.Keywords;

            // Ensure _selectedKeywordsIndex has the correct size
            if (SelectedKeywordsIndex == null || SelectedKeywordsIndex.Length != SelectedKeywords.Length)
            {
                SelectedKeywordsIndex = new int[SelectedKeywords.Length];
            }

            // Populate the indices for the selected keywords
            for (int i = 0; i < SelectedKeywords.Length; i++)
            {
                if (KeywordNamesList != null)
                {
                    SelectedKeywordsIndex[i] = KeywordNamesList.IndexOf(SelectedKeywords[i].keywordName);
                }
                else
                {
                    Debug.LogError("Keyword names list is null");
                    SelectedKeywordsIndex[i] = -1; // or another default/failure value
                }
            }
        }
        
        private void GetStatsFromLoadedCard()
        {
            switch (CardTypes)
            {
                case CardTypes.TBD:
                case CardTypes.Starship:
                case CardTypes.Action:
                    break;
                case CardTypes.Environment:
                    ExploreStat = SelectedCard.Explore;
                    _exploreValue = SelectedCard.Explore.StatValue;
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                case CardTypes.Character_Ally:
                case CardTypes.Character_Hunter:
                case CardTypes.Boss:
                case CardTypes.Creature:
                    AttackStat = SelectedCard.Attack;
                    _attackValue = SelectedCard.Attack.StatValue;
                    HitPointsStat = SelectedCard.HitPoints;
                    _hitPointsValue = SelectedCard.HitPoints.StatValue;
                    SpeedStat = SelectedCard.Speed;
                    _speedValue = SelectedCard.Speed.StatValue;
                    FocusStat = SelectedCard.Focus;
                    _focusValue = SelectedCard.Focus.StatValue;
                    if (CardTypes == CardTypes.Character_Hunter)
                    {
                        UpgradeSlotsStat = SelectedCard.UpgradeSlots;
                        _upgradeSlotsValue = SelectedCard.UpgradeSlots.StatValue;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            } 
        }
        
        private void RefreshCardList()
        {
            if (!ReferenceEquals(SelectedCard, null))
            {
                UnloadCard();
            }
            AllCards.Clear();
            SelectedCards.Clear();
            string[] guids = AssetDatabase.FindAssets(ASSET_FILTER, new[] { ASSET_PATH });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                CardSO card = ErrorHandler.TryToGetCard(AssetDatabase.LoadAssetAtPath<CardSO>(assetPath));
                if (ErrorHandler.VerifyCardNotInList(AllCards, card))
                {
                    AllCards.Add(card);
                    SelectedCards[card] = false;
                }
            }
            AllCards.Sort((card1, card2) =>
            {
                int compare = card1.CardType.CompareTo(card2.CardType);

                return compare == 0 ? string.Compare(card1.CardName, card2.CardName, StringComparison.Ordinal) : compare;
            });
        }
        
        private void UnloadCard()
        {
            CardSO card = ErrorHandler.TryToGetCard(SelectedCard);
            CardStat blankStat = new(StatNames.None);
            SetSelectedCard(card,false);
            CardTypes = CardTypes.TBD;
            CardName = string.Empty;
            CardRarity = CardRarity.None;
            CardCost = 0;
            CardText = string.Empty;
            AttackStat = blankStat;
            HitPointsStat = blankStat;
            SpeedStat = blankStat;
            FocusStat = blankStat;
            ExploreStat = blankStat;
            UpgradeSlotsStat = blankStat;
            SelectedKeywords = Array.Empty<Keyword>();
            Artwork = _placeholderArtwork;
        }
        
        private void SetSelectedCard(CardSO card, bool isSelected)
        {
           ErrorHandler.TryToGetCard(card);
           if (!ReferenceEquals(SelectedCard, null))
           {
               SelectedCards[SelectedCard] = false;
           }
           foreach (CardSO key in SelectedCards.Keys.ToList())
           {
               SelectedCards[key] = false;
           }
           SelectedCard = card;
           SelectedCards[card] = isSelected;
        }

        
        private void SaveExistingCard()
        {
            Undo.RecordObject(SelectedCard, "Edit Card");
            if (string.IsNullOrEmpty(CardName))
            {
                return;
            }

            InitializeCard(SelectedCard);
            SetWeightData(SelectedCard);
            EditorUtility.SetDirty(SelectedCard);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void ChangeCardAssetFileName()
        {
            if (CardName == SelectedCard.CardName)
            {
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(SelectedCard);
            AssetDatabase.RenameAsset(assetPath, CardName);
            RefreshCardList();
        }
        
        private void InitializeCard(CardSO card)
        {
            ErrorHandler.TryToGetCard(card);
            ChangeCardAssetFileName();
            InitializeCommonCardData(card);
            AssignStatsToCard(card);
            SetWeightData(card);
        }

        private void InitializeCommonCardData(CardSO card)
        {
            card.CardType = CardTypes;
            card.Rarity = CardRarity;
            card.CardName = CardName;
            card.ArtWork = Artwork;
            card.Keywords = SelectedKeywords;
            card.CardText = CardText;
            card.CardCost = CardCost;
        }
        private void AssignStatsToCard(CardSO card)
        {
            switch (CardTypes)
            {
                case CardTypes.TBD:
                case CardTypes.Starship:
                case CardTypes.Action:
                    break;
                case CardTypes.Environment:
                    card.Explore = new CardStat(StatNames.Explore, _exploreValue, EXPLORE_DESCRIPTION);
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                case CardTypes.Character_Ally:
                case CardTypes.Character_Hunter:
                case CardTypes.Creature:
                case CardTypes.Boss:
                    card.Attack = new CardStat(StatNames.Attack, _attackValue, ATTACK_DESCRIPTION);
                    card.HitPoints = new CardStat(StatNames.HP, _hitPointsValue, HIT_POINTS_DESCRIPTION);
                    card.Speed = new CardStat(StatNames.Speed, _speedValue, SPEED_DESCRIPTION);
                    card.Focus = new CardStat(StatNames.Focus, _focusValue, FOCUS_DESCRIPTION);
                    if (CardTypes == CardTypes.Character_Hunter)
                    {
                        card.UpgradeSlots = new CardStat(StatNames.Upgrades, _upgradeSlotsValue, UPGRADE_SLOTS_DESCRIPTION);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Card Type does not exixt or is not implemented.");
            }
        }
        private void SetWeightData(CardSO card)
        {
            WeightContainer weights = GetWeightContainer(CardTypes);
            ErrorHandler.TryToGetWeightData(card.WeightData);
            card.WeightData = weights;
        }
        
        private WeightContainer GetWeightContainer(CardTypes cardType)
        {
            return cardType switch
            {
                CardTypes.Character_Ally => AssetDatabase.LoadAssetAtPath<WeightContainer>(ALLY_WEIGHT_ASSET_PATH),
                CardTypes.Environment => AssetDatabase.LoadAssetAtPath<WeightContainer>(ENVIRONMENT_WEIGHT_ASSET_PATH),
                CardTypes.Creature => AssetDatabase.LoadAssetAtPath<WeightContainer>(CREATURE_WEIGHT_ASSET_PATH),
                CardTypes.Boss => AssetDatabase.LoadAssetAtPath<WeightContainer>(BOSS_WEIGHT_ASSET_PATH),
                CardTypes.Character_Hunter => AssetDatabase.LoadAssetAtPath<WeightContainer>(HUNTER_WEIGHT_ASSET_PATH),
                CardTypes.Gear_Equipment=> AssetDatabase.LoadAssetAtPath<WeightContainer>(GEAR_WEIGHT_ASSET_PATH),
                CardTypes.Gear_Upgrade => AssetDatabase.LoadAssetAtPath<WeightContainer>(GEAR_WEIGHT_ASSET_PATH),
                _ => AssetDatabase.LoadAssetAtPath<WeightContainer>(KEYWORD_ONLY_WEIGHT_ASSET_PATH)
            };
        }
    }
}