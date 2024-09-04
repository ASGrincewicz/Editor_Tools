using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.AttributesWeights;
using Editor.CostCalculator;
using Editor.KeywordSystem;
using Editor.Utilities;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static Editor.CardEditor.StatDataReference;

namespace Editor.CardEditor
{
    public class CardEditorWindow : EditorWindow
    {
        [SerializeField] private KeywordManager _keywordManager;
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
        private List<CardSO> _allCards = new ();
        private Dictionary<CardSO, bool> _selectedCards = new ();
        private CardSO _cardToEdit;
        
        // Method specific variables
        private StringBuilder _stringBuilder;
        private CardTypes _cardTypes;
        private string _cardName;
        private CardStat? _attack;
        private int _attackValue;
        private CardStat? _explore;
        private int _exploreValue;
        private CardStat? _focus;
        private int _focusValue;
        private CardStat? _hitPoints;
        private int _hitPointsValue;
        private CardStat? _speed;
        private int _speedValue;
        private CardStat? _upgradeSlots;
        private int _upgradeSlotsValue;
        [CanBeNull] private Texture2D _artwork;
        private List<Keyword> _keywordsList;
        private List<string> _keywordNamesList;
        private Keyword[] _selectedKeywords;
        private int[] _selectedKeywordsIndex;
        [Multiline] private string _cardText;
        private int _cardCost;
        private CardSO _selectedCard;
        
        
        // GUI variables
        private Vector2 _scrollPosition;
        private Vector2 _scrollPosition2;
        private Rect _mainAreaRect;
        private Rect _secondAreaRect;

        [MenuItem("Tools/Card Editor")]
        public static void Init()
        {
            EditorWindow window = GetWindow<CardEditorWindow>("Card Editor");
            window.position = new Rect(50f, 50f, 700f, 800f);
            window.Show();
        }
        
        private void OnEnable()
        {
            string[] guids = AssetDatabase.FindAssets(ASSET_FILTER, new[] { ASSET_PATH });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                CardSO card = AssetDatabase.LoadAssetAtPath<CardSO>(assetPath);
                if (card != null)
                {
                    if (!_allCards.Contains(card))
                    {
                        _allCards.Add(card);
                    }
                    _selectedCards[card] = false; // Initialize all cards as unselected
                }
            }
            RefreshKeywordsList();
            
            _stringBuilder = new StringBuilder();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawMainArea();
            DrawCardListArea();
        }

        public void OpenCardInEditor(CardSO card)
        {
            _cardToEdit = card;
            LoadCardFromFile();
        }

        private void SetupAreaRects()
        {
            _mainAreaRect= new Rect(5,5,position.width - 10,position.height - 225);
            _secondAreaRect = new Rect(5, _mainAreaRect.y + _mainAreaRect.height + 20, position.width - 10, position.height - _mainAreaRect.height - 50);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(_mainAreaRect);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawEditableFields();
            DrawControlButtons();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawEditableFields()
        {
            EditorGUIUtility.labelWidth = 100;
            _cardToEdit = EditorGUILayout.ObjectField("Card To Edit",_cardToEdit, typeof(CardSO),false) as CardSO;
            GUILayout.Label(!ReferenceEquals(_selectedCard, null) ? "Select Card Type" : "Create New Card",
                EditorStyles.boldLabel);
            _cardTypes = (CardTypes)EditorGUILayout.EnumPopup("Card Type",_cardTypes, GUILayout.Width(FIELD_WIDTH));
           
            _cardName = EditorGUILayout.TextField("Card Name", _cardName, GUILayout.Width(FIELD_WIDTH));
            _artwork = (Texture2D)EditorGUILayout.ObjectField("Artwork", _artwork, typeof(Texture2D), false,
                GUILayout.Height(200), GUILayout.Width(FIELD_WIDTH));

            switch (_cardTypes)
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
                    if (_cardTypes == CardTypes.Character_Hunter) DrawStatLayout(StatNames.Upgrades, ref _upgradeSlotsValue, UPGRADE_SLOTS_DESCRIPTION);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
          
            DrawKeywordArea();
            GUILayout.Label($"Card Cost: {_cardCost}");
            GUILayout.Label("Card Text");
            _cardText = EditorGUILayout.TextArea(_cardText, GUILayout.Height(100), GUILayout.Width(FIELD_WIDTH));
        }

        private void DrawKeywordArea()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH));
            EditorGUIUtility.labelWidth = 100;
            GUILayout.Label("Keywords");
            if (_selectedKeywords is not { Length: 3 })
            {
                _selectedKeywords = new Keyword[3];
                _selectedKeywordsIndex = new int[3]; // Initialize the index array only once.
            }

            for (int i = 0; i < _selectedKeywords.Length; i++)
            {
                _selectedKeywordsIndex[i] = EditorGUILayout.Popup(
                    _selectedKeywordsIndex[i],
                    _keywordNamesList.ToArray(),
                    GUILayout.Width(FIELD_WIDTH / 3)
                );
                //TODO: Need error checking on this.
                // Use Find method to assign Keyword to _selectedKeywords
                string selectedKeywordName = _keywordNamesList[_selectedKeywordsIndex[i]];
                _selectedKeywords[i] = _keywordManager.GetKeywordByName(selectedKeywordName);
            }
          
            GUILayout.EndHorizontal();
        }

        private void DrawControlButtons()
        {
            using GUILayout.HorizontalScope horizontalScope = new();
            if (ReferenceEquals(_selectedCard, null) )
            {
                if (GUILayout.Button("Create Card"))
                {
                    CreateNewCard();
                }
                if (GUILayout.Button("Load Card From File"))
                {
                    LoadCardFromFile();
                }
            }
            else
            {
                if (GUILayout.Button("Save Changes"))
                {
                    SaveExistingCard();
                }
                if(GUILayout.Button("Unload Card") && !ReferenceEquals(_selectedCard, null))
                {
                    UnloadCard();
                }

                if (GUILayout.Button("Calculate Cost") && !ReferenceEquals(_selectedCard, null))
                {
                    CostCalculatorWindow instance = EditorWindow.GetWindow<CostCalculatorWindow>();
                    instance.OpenInCostCalculatorWindow(_selectedCard);
                }
            }
        }

        private void DrawCardListArea()
        {
            GUILayout.BeginArea(_secondAreaRect);
            _scrollPosition2 = GUILayout.BeginScrollView(_scrollPosition2, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
    
            GUILayout.Label("Card List", EditorStyles.boldLabel);
    
            const int ColumnsDesired = 3;
            int columnCounter = 0;
    
            foreach (CardSO card in _allCards)
            {
                if (columnCounter % ColumnsDesired == 0)
                {
                    GUILayout.BeginHorizontal();
                }
        
                GUILayout.BeginVertical(GUILayout.Width(_secondAreaRect.width / ColumnsDesired));
                _selectedCards[card] = EditorGUILayout.ToggleLeft($"{card.CardName}/{card.CardType}", _selectedCards[card], GUILayout.ExpandWidth(true));
                GUILayout.EndVertical();

                if (columnCounter % ColumnsDesired == ColumnsDesired - 1)
                {
                    GUILayout.EndHorizontal();
                }
        
                columnCounter++;
            }

            if (columnCounter % ColumnsDesired != 0) // Close the last row if it doesn't have ColumnsDesired items
            {
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
    
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Edit Selected Card", GUILayout.ExpandWidth(true)))
            {
                EditSelectedCard();
            }
            if (GUILayout.Button("Refresh Keywords", GUILayout.ExpandWidth(true)))
            {
               RefreshKeywordsList();
            }
            if (GUILayout.Button("Refresh Card List", GUILayout.ExpandWidth(true)))
            {
               RefreshCardList(true);
            }
            GUILayout.EndHorizontal();
    
            GUILayout.EndArea();
        }
        
        private bool InitializeCard(CardSO card)
        {
            if (ReferenceEquals(card, null))
            {
                return false;
            }
            card.CardType = _cardTypes;
            card.CardName = _cardName;
            card.ArtWork = _artwork;
            card.Keywords = _selectedKeywords;
            card.CardText = _cardText;
            card.CardCost = _cardCost;
            switch (_cardTypes)
            {
                case CardTypes.TBD:
                    break;
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
                    if (_cardTypes == CardTypes.Character_Hunter)
                    {
                        card.UpgradeSlots = new CardStat(StatNames.Upgrades, _upgradeSlotsValue, UPGRADE_SLOTS_DESCRIPTION);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetWeightData(card);
            return true;
        }

        private void CreateNewCard()
        {
            CardSO newCard = CreateInstance<CardSO>();
            if (InitializeCard(newCard))
            {
                AssetDatabase.CreateAsset(newCard, $"{ASSET_PATH}{_cardName}.asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newCard;
                RefreshCardList(true);
                _cardToEdit = newCard;
            }
            else
            {
                Debug.Log("Initialization Failed: Card is null.");
            }
        }
        // Method to Get Weight Container based on Card Type
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

        private void SetWeightData(CardSO card)
        {
            WeightContainer weights = GetWeightContainer(_cardTypes);
            if (ReferenceEquals(card.WeightData, null))
            {
                Debug.LogError($"{nameof(card.WeightData)} is null.");
            }
            card.WeightData = weights;
        }
        private void UpdateSelectedKeywordsIndices()
        {
            // Check if _selectedCard and its Keywords are not null
            if (_selectedCard?.Keywords == null)
            {
                Debug.LogError("Selected card or its keywords are null");
                return;
            }

            _selectedKeywords = _selectedCard.Keywords;

            // Ensure _selectedKeywordsIndex has the correct size
            if (_selectedKeywordsIndex == null || _selectedKeywordsIndex.Length != _selectedKeywords.Length)
            {
                _selectedKeywordsIndex = new int[_selectedKeywords.Length];
            }

            // Populate the indices for the selected keywords
            for (int i = 0; i < _selectedKeywords.Length; i++)
            {
                if (_keywordNamesList != null)
                {
                    _selectedKeywordsIndex[i] = _keywordNamesList.IndexOf(_selectedKeywords[i].keywordName);
                }
                else
                {
                    Debug.LogError("Keyword names list is null");
                    _selectedKeywordsIndex[i] = -1; // or another default/failure value
                }
            }
        }

        private void LoadCardFromFile()
        {
            UnloadCard();
            if (!ReferenceEquals(_cardToEdit, null))
            {
                _selectedCard = _cardToEdit;
            }
            else
            {
                string path = EditorUtility.OpenFilePanel("Select Card", ASSET_PATH, "asset");
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path[Application.dataPath.Length..];
                    _selectedCard = AssetDatabase.LoadAssetAtPath<CardSO>(path);
                    _cardToEdit = _selectedCard;
                }
            }
            
            if (!ReferenceEquals(_selectedCard, null))
            {
                _cardTypes = _selectedCard.CardType;
                _cardName = _selectedCard.CardName;
                _artwork = _selectedCard.ArtWork;
                _cardCost = _selectedCard.CardCost;
                
               UpdateSelectedKeywordsIndices();
                _cardText += _stringBuilder.Append(_selectedCard.CardText);
                switch (_cardTypes)
                {
                    case CardTypes.TBD:
                    case CardTypes.Starship:
                    case CardTypes.Action:
                    break;
                    case CardTypes.Environment:
                    _explore = _selectedCard.Explore;
                    _exploreValue = _selectedCard.Explore.StatValue;
                    break;
                    case CardTypes.Gear_Equipment:
                    case CardTypes.Gear_Upgrade:
                    case CardTypes.Character_Ally:
                    case CardTypes.Character_Hunter:
                    case CardTypes.Boss:
                    case CardTypes.Creature:
                        _attack = _selectedCard.Attack;
                        _attackValue = _selectedCard.Attack.StatValue;
                        _hitPoints = _selectedCard.HitPoints;
                        _hitPointsValue = _selectedCard.HitPoints.StatValue;
                        _speed = _selectedCard.Speed;
                        _speedValue = _selectedCard.Speed.StatValue;
                        _focus = _selectedCard.Focus;
                        _focusValue = _selectedCard.Focus.StatValue;
                    if (_cardTypes == CardTypes.Character_Hunter)
                        {
                            _upgradeSlots = _selectedCard.UpgradeSlots;
                            _upgradeSlotsValue = _selectedCard.UpgradeSlots.StatValue;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               
                _stringBuilder.Clear();;
            }
        }

        private void UnloadCard()
        {
            _selectedCard = null;
            _cardTypes = CardTypes.TBD;
            _cardName = string.Empty;
            _cardText = string.Empty;
            _attack = null;
            _hitPoints = null;
            _speed = null;
            _focus = null;
            _explore = null;
            _upgradeSlots = null;
            _selectedKeywords = null;
            _artwork = null;
        }

        private void SaveExistingCard()
        {
            Undo.RecordObject(_selectedCard, "Edited Card");
            if (InitializeCard(_selectedCard))
            {
                SetWeightData(_selectedCard);
                EditorUtility.SetDirty(_selectedCard);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        private void EditSelectedCard()
        {
            // Perform editing on selected cards
            foreach (KeyValuePair<CardSO, bool> entry in _selectedCards)
            {
                if (entry.Value) // If the card is selected
                {
                    _cardToEdit = entry.Key;
                    LoadCardFromFile();
                }
            }
            _stringBuilder.Clear();
        }

        private void RefreshCardList(bool clearList)
        {
            UnloadCard();
            if (clearList)
            {
                _allCards.Clear();
                _selectedCards.Clear();
            }
            string[] guids = AssetDatabase.FindAssets(ASSET_FILTER, new[] { ASSET_PATH });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                CardSO card = AssetDatabase.LoadAssetAtPath<CardSO>(assetPath);
                if (!ReferenceEquals(card, null))
                {
                    _allCards.Add(card);
                    _selectedCards[card] = false; // Initialize all cards as unselected
                }
            }
            _allCards.Sort((card1, card2) =>
            {
                int compare = card1.CardType.CompareTo(card2.CardType);

                return compare == 0 ? string.Compare(card1.CardName, card2.CardName, StringComparison.Ordinal) : compare;
            });
        }

        private void RefreshKeywordsList()
        {
            _keywordsList = new List<Keyword>();
            foreach (Keyword keyword in _keywordManager.keywordList)
            {
                if (!keyword.IsDefault())
                {
                    _keywordsList.Add(keyword);
                }
            }
            _keywordNamesList = new List<string>();
            foreach (Keyword keyword in _keywordsList.ToList())
            {
                if (!keyword.IsDefault())
                {
                    _keywordNamesList.Add(keyword.keywordName);
                }
            }
        }
        private void DrawStatLayout(StatNames statName, ref int statValue, string statDescription)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(100));
            GUILayout.Label(statName.GetDescription());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(100));
            statValue = EditorGUILayout.IntField(" Value", statValue,GUILayout.Width(130),GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.Label(statDescription);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }  
        
    }
}