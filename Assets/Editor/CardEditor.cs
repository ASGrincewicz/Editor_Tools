using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Data.Scriptable_Objects;
using UnityEditor;
using UnityEngine;
using static Editor.StatDataReference;

namespace Editor
{
    public class CardEditor : EditorWindow
    {
        private const string ASSET_PATH = "Assets/Data/Scriptable Objects/Cards/";
        private const string ASSET_FILTER = "t:CardSO";
        private List<CardSO> allCards = new ();
        private Dictionary<CardSO, bool> selectedCards = new ();
        private Vector2 scrollPosition;
        private Vector2 scrollPosition2;
        private GUIContent dropDownContent;
        private StringBuilder _stringBuilder;

        // Temporary variables to hold the values to apply
        private CardSO _cardToEdit;
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
        private const float FIELD_WIDTH = 400;
        
        //Positioning
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
            mainAreaRect= new Rect(5,5,position.width - 10,position.height - 225);
            secondAreaRect = new Rect(5, mainAreaRect.y + mainAreaRect.height + 20, position.width - 10, position.height - mainAreaRect.height - 50);
            GUILayout.BeginArea(mainAreaRect);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
                    //Explore Stat Layout Start
                    GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    GUILayout.Label(EXPLORE_NAME);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    _exploreValue = EditorGUILayout.IntField(" Value", _exploreValue,GUILayout.Width(130),GUILayout.ExpandWidth(true));
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(200));
                    GUILayout.Label(EXPLORE_DESCRIPTION);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    //Explore Stat Layout end
                    break;
                case CardTypes.Equipment:
                    break;
                case CardTypes.Hunter:
                case CardTypes.Character:
                case CardTypes.Creature:
                case CardTypes.Boss:
                    //Attack Stat Layout Start
                    GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    GUILayout.Label(ATTACK_NAME);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    _attackValue = EditorGUILayout.IntField(" Value", _attackValue,GUILayout.Width(130),GUILayout.ExpandWidth(true));
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(200));
                    GUILayout.Label(ATTACK_DESCRIPTION);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    //Attack Stat Layout end
                    //Hit Points Stat Layout Start
                    GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    GUILayout.Label(HIT_POINTS_NAME);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    _hitPointsValue = EditorGUILayout.IntField("Value", _hitPointsValue,GUILayout.Width(130), GUILayout.ExpandWidth(true));
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(200));
                    GUILayout.Label(HIT_POINTS_DESCRIPTION);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    //Hit Points Stat Layout End
                    //Speed Stat Layout Start
                    GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    GUILayout.Label(SPEED_NAME);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    _speedValue = EditorGUILayout.IntField("Value", _speedValue,GUILayout.Width(130), GUILayout.ExpandWidth(true));
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(200));
                    GUILayout.Label(SPEED_DESCRIPTION);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    //Speed Stat Layout End
                    //Focus Stat Layout Start
                    GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    GUILayout.Label(FOCUS_NAME);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(100));
                    _focusValue = EditorGUILayout.IntField("Value", _focusValue,GUILayout.Width(130), GUILayout.ExpandWidth(true));
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(200));
                    GUILayout.Label(FOCUS_DESCRIPTION);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    //Focus Stat Layout End
                    if (_cardTypes == CardTypes.Hunter)
                    {
                        //Upgrade Slots Stat Layout Start
                        GUILayout.BeginHorizontal(GUILayout.Width(FIELD_WIDTH),GUILayout.ExpandWidth(true));
                        GUILayout.BeginVertical(GUILayout.Width(100));
                        GUILayout.Label(UPGRADE_SLOTS_NAME);
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical(GUILayout.Width(100));
                        _upgradeSlotsValue = EditorGUILayout.IntField("Value", _upgradeSlotsValue,GUILayout.Width(130), GUILayout.ExpandWidth(true));
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical(GUILayout.Width(200));
                        GUILayout.Label(UPGRADE_SLOTS_DESCRIPTION);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                        //Hit Points Stat Layout End
                    }
                    break;
                case CardTypes.Upgrade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            /*// start creating new elements
            newStat.StatName = EditorGUILayout.TextField("Stat Name", newStat.StatName, GUILayout.Width(FIELD_WIDTH));
            newStat.StatValue = EditorGUILayout.IntField("Stat Value", newStat.StatValue,GUILayout.Width(200));
            newStat.StatDescription = EditorGUILayout.TextField("Stat Description", newStat.StatDescription, GUILayout.Width(FIELD_WIDTH));

            if (GUILayout.Button("Add Stat", GUILayout.Width(FIELD_WIDTH)))
            {
                // When the user clicks the "Add Stat" button add the new stat to the list of stats
                cardStats.Add(newStat);
                newStat = new CardStat();
            }
            
            // show all stats 
            EditorGUILayout.LabelField("Card Stats:");
            if (cardStats != null)
            {
                for (int i = 0; i < cardStats.Count; i++)
                {
                    CardStat stat = cardStats[i];

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 50;
                    EditorGUILayout.PrefixLabel("Stat " + (i+1));
                

                    // allow users to edit the stat values directly in the list
                    EditorGUILayout.BeginVertical(GUILayout.Width(200));
                    stat.StatName = EditorGUILayout.TextField("Name: ", stat.StatName, GUILayout.Width(200));
                    EditorGUILayout.EndVertical();
                
                    EditorGUILayout.BeginVertical(GUILayout.Width(100));
                    stat.StatValue = EditorGUILayout.IntField("Value: ", stat.StatValue,GUILayout.Width(100));
                    EditorGUILayout.EndVertical();
                
                    EditorGUILayout.BeginVertical(GUILayout.Width(200));
                    stat.StatDescription = EditorGUILayout.TextArea(stat.StatDescription);
                    EditorGUILayout.EndVertical();
                

                    // update the list with the edited stat
                    cardStats[i] = stat;

                    // provide a button to remove this stat from the list
                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        cardStats.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }*/
            
            
            GUILayout.Label("Card Text");
            cardText = EditorGUILayout.TextArea(cardText, GUILayout.Height(100), GUILayout.Width(FIELD_WIDTH));
            // Add other stats and text fields
            
           
            

            GUILayout.EndScrollView();
            using (var horizontalScope = new GUILayout.HorizontalScope())
            {
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
            GUILayout.EndArea();
           // GUILayout.Space(50.0f);
            
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
            using (var horizontalScope = new GUILayout.HorizontalScope())
            {
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
                        _hitPoints = selectedCard.HitPoints;
                        _speed = selectedCard.Speed;
                        _focus = selectedCard.Focus;
                    if (_cardTypes == CardTypes.Hunter)
                        {
                            _upgradeSlots = selectedCard.UpgradeSlots;
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
            //cardStats = new List<CardStat>();
            //Todo: Set all stats to null.
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
                //InitializeCard(selectedCard);
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
                    //EditorUtility.SetDirty(entry.Key);
                }
            }
            _stringBuilder.Clear();
            //AssetDatabase.SaveAssets(); 
            //AssetDatabase.Refresh();
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
    }
}