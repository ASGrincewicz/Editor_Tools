using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using static Editor.CardEditor.StatDataReference;

namespace Editor.CardEditor
{
    public class CardEditor : EditorWindow
    {
        // Constants
        private const string ASSET_PATH = "Assets/Data/Scriptable Objects/Cards/";
        private const string ASSET_FILTER = "t:CardSO";
        private const float FIELD_WIDTH = 400;
        
        // Class variables
        private List<CardSO> allCards = new ();
        private Dictionary<CardSO, bool> selectedCards = new ();
        private CardSO _cardToEdit;
        
        // Method specific variables
        private StringBuilder _stringBuilder;
        private CardTypes _cardTypes;
        private string cardName;
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
        private Texture2D artwork;
        [Multiline] private string cardText;
        private CardSO selectedCard;
        
        
        // GUI variables
        private Vector2 scrollPosition;
        private Vector2 scrollPosition2;
        private Rect mainAreaRect;
        private Rect secondAreaRect;

        [MenuItem("Tools/Card Editor")]
        public static void Init()
        {
            EditorWindow window = GetWindow<CardEditor>("Card Editor");
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
                    if (!allCards.Contains(card))
                    {
                        allCards.Add(card);
                    }
                    selectedCards[card] = false; // Initialize all cards as unselected
                }
            }
            _stringBuilder = new StringBuilder();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawMainArea();
            DrawBatchEditArea();
        }

        public void OpenCardInEditor(CardSO card)
        {
            _cardToEdit = card;
            LoadCardFromFile();
        }

        private void SetupAreaRects()
        {
            mainAreaRect= new Rect(5,5,position.width - 10,position.height - 225);
            secondAreaRect = new Rect(5, mainAreaRect.y + mainAreaRect.height + 20, position.width - 10, position.height - mainAreaRect.height - 50);
        }

        private void DrawMainArea()
        {
            GUILayout.BeginArea(mainAreaRect);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawEditableFields();
            DrawControlButtons();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawEditableFields()
        {
            EditorGUIUtility.labelWidth = 100;
            _cardToEdit = EditorGUILayout.ObjectField("Card To Edit",_cardToEdit, typeof(CardSO),false) as CardSO;
            GUILayout.Label(!ReferenceEquals(selectedCard, null) ? "Select Card Type" : "Create New Card",
                EditorStyles.boldLabel);
            _cardTypes = (CardTypes)EditorGUILayout.EnumPopup("Card Type",_cardTypes, GUILayout.Width(FIELD_WIDTH));
           
            cardName = EditorGUILayout.TextField("Card Name", cardName, GUILayout.Width(FIELD_WIDTH));
            artwork = (Texture2D)EditorGUILayout.ObjectField("Artwork", artwork, typeof(Texture2D), false,
                GUILayout.Height(200), GUILayout.Width(FIELD_WIDTH));

            switch (_cardTypes)
            {
                case CardTypes.TBD:
                    break;
                case CardTypes.Action:
                    break;
                case CardTypes.Environment:
                    DrawStatLayout(EXPLORE_NAME, ref _exploreValue, EXPLORE_DESCRIPTION);
                    break;
                case CardTypes.Equipment:
                    break;
                case CardTypes.Hunter:
                case CardTypes.Character:
                case CardTypes.Creature:
                case CardTypes.Boss:
                    DrawStatLayout(ATTACK_NAME, ref _attackValue, ATTACK_DESCRIPTION);
                    DrawStatLayout(HIT_POINTS_NAME, ref _hitPointsValue, HIT_POINTS_DESCRIPTION);
                    DrawStatLayout(SPEED_NAME, ref _speedValue, SPEED_DESCRIPTION);
                    DrawStatLayout(FOCUS_NAME, ref _focusValue, FOCUS_DESCRIPTION);
                    if (_cardTypes == CardTypes.Hunter) DrawStatLayout(UPGRADE_SLOTS_NAME, ref _upgradeSlotsValue, UPGRADE_SLOTS_DESCRIPTION);
                    break;
                case CardTypes.Upgrade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
           
            GUILayout.Label("Card Text");
            cardText = EditorGUILayout.TextArea(cardText, GUILayout.Height(100), GUILayout.Width(FIELD_WIDTH));
        }

        private void DrawControlButtons()
        {
            using GUILayout.HorizontalScope horizontalScope = new();
            if (ReferenceEquals(selectedCard, null) )
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
                if(GUILayout.Button("Unload Card") && !ReferenceEquals(selectedCard, null))
                {
                    UnloadCard();
                }
            }
        }

        private void DrawBatchEditArea()
        {
            GUILayout.BeginArea(secondAreaRect);
            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            GUILayout.Label("Batch Editing", EditorStyles.boldLabel);
            // Display all cards with checkboxes
            allCards.Sort((card1, card2) => card1.CardType.CompareTo(card2.CardType));
            foreach (CardSO card in allCards)
            {
                selectedCards[card] = EditorGUILayout.ToggleLeft($"{card.CardType}/{card.CardName}", selectedCards[card]);
            }
            GUILayout.EndScrollView();
            using GUILayout.HorizontalScope horizontalScope = new();
            if (GUILayout.Button("Edit Selected Card"))
            {
                EditSelectedCard();
            }

            if (GUILayout.Button("Select All"))
            {
                SelectAllCards(true);
            }
            
            if (GUILayout.Button("Deselect All"))
            {
                SelectAllCards(false);
            }

            GUILayout.EndArea();
        }
        
        private bool InitializeCard(CardSO card)
        {
            if (ReferenceEquals(card, null))
            {
                return false;
            }
            card.CardType = _cardTypes;
            card.CardName = cardName;
            card.ArtWork = artwork;
            card.CardText = cardText;
            switch (_cardTypes)
            {
                case CardTypes.TBD:
                    break;
                case CardTypes.Action:
                    break;
                case CardTypes.Environment:
                    _explore = new CardStat(EXPLORE_NAME, _exploreValue, EXPLORE_DESCRIPTION);
                    break;
                case CardTypes.Equipment:
                    break;
                case CardTypes.Hunter:
                case CardTypes.Boss:
                case CardTypes.Character:
                case CardTypes.Creature:
                    card.Attack = new CardStat(ATTACK_NAME, _attackValue, ATTACK_DESCRIPTION);
                    card.HitPoints = new CardStat(HIT_POINTS_NAME, _hitPointsValue, HIT_POINTS_DESCRIPTION);
                    card.Speed = new CardStat(SPEED_NAME, _speedValue, SPEED_DESCRIPTION);
                    card.Focus = new CardStat(FOCUS_NAME, _focusValue, FOCUS_DESCRIPTION);
                    if (_cardTypes == CardTypes.Hunter)
                    {
                        card.UpgradeSlots = new CardStat(UPGRADE_SLOTS_NAME, _upgradeSlotsValue, UPGRADE_SLOTS_DESCRIPTION);
                    }
                    break;
                case CardTypes.Upgrade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        private void CreateNewCard()
        {
            CardSO newCard = CreateInstance<CardSO>();
            if (InitializeCard(newCard))
            {
                AssetDatabase.CreateAsset(newCard, $"{ASSET_PATH}{cardName}.asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newCard;
                RefreshCardList(true);
            }
            else
            {
                Debug.Log("Initialization Failed: Card is null.");
            }
        }

        private void LoadCardFromFile()
        {
            UnloadCard();
            if (_cardToEdit != null)
            {
                selectedCard = _cardToEdit;
            }
            else
            {
                string path = EditorUtility.OpenFilePanel("Select Card", ASSET_PATH, "asset");
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path[Application.dataPath.Length..];
                    selectedCard = AssetDatabase.LoadAssetAtPath<CardSO>(path);
                    _cardToEdit = selectedCard;
                }
            }
            
            if (!ReferenceEquals(selectedCard, null))
            {
                _cardTypes = selectedCard.CardType;
                cardName = selectedCard.CardName;
                artwork = selectedCard.ArtWork;
                cardText += _stringBuilder.Append(selectedCard.CardText);
                switch (_cardTypes)
                {
                    case CardTypes.TBD:
                    break;
                    case CardTypes.Action:
                    break;
                    case CardTypes.Environment:
                    _explore = selectedCard.Explore;
                    break;
                    case CardTypes.Equipment:
                    break;
                    case CardTypes.Hunter:
                    case CardTypes.Boss:
                    case CardTypes.Character:
                    case CardTypes.Creature:
                        _attack = selectedCard.Attack;
                        _attackValue = selectedCard.Attack.StatValue;
                        _hitPoints = selectedCard.HitPoints;
                        _hitPointsValue = selectedCard.HitPoints.StatValue;
                        _speed = selectedCard.Speed;
                        _speedValue = selectedCard.Speed.StatValue;
                        _focus = selectedCard.Focus;
                        _focusValue = selectedCard.Focus.StatValue;
                    if (_cardTypes == CardTypes.Hunter)
                        {
                            _upgradeSlots = selectedCard.UpgradeSlots;
                            _upgradeSlotsValue = selectedCard.UpgradeSlots.StatValue;
                        }
                        break;
                    case CardTypes.Upgrade:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               
                _stringBuilder.Clear();;
            }
        }

        private void UnloadCard()
        {
            selectedCard = null;
            _cardTypes = CardTypes.TBD;
            cardName = string.Empty;
            cardText = string.Empty;
            _attack = null;
            _hitPoints = null;
            _speed = null;
            _focus = null;
            _explore = null;
            _upgradeSlots = null;
            
            artwork = null;
        }

        private void SaveExistingCard()
        {
            Undo.RecordObject(selectedCard, "Edited Card");
            if (InitializeCard(selectedCard))
            {
                EditorUtility.SetDirty(selectedCard);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            UnloadCard();
            SelectAllCards(false);
        }

        private void SelectAllCards(bool select)
        {
            foreach (CardSO card in selectedCards.Keys.ToList())
            {
               selectedCards[card] = select;
            }
        }
        private void EditSelectedCard()
        {
            // Perform editing on selected cards
            foreach (KeyValuePair<CardSO, bool> entry in selectedCards)
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
                allCards.Clear();
                selectedCards.Clear();
            }
            string[] guids = AssetDatabase.FindAssets(ASSET_FILTER, new[] { ASSET_PATH });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                CardSO card = AssetDatabase.LoadAssetAtPath<CardSO>(assetPath);
                if (!ReferenceEquals(card, null))
                {
                    allCards.Add(card);
                    selectedCards[card] = false; // Initialize all cards as unselected
                }
            }
        }
        private void DrawStatLayout(string statName, ref int statValue, string statDescription)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(100));
            GUILayout.Label(statName);
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