using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Data.Scriptable_Objects;
using UnityEditor;
using UnityEngine;

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
        private readonly string[] CARD_TYPES =
            { "TBD", "Action", "Boss", "Character", "Creature", "Equipment", "Environment", "Hunter", "Upgrade" };
        private string cardType;
        private string cardName;
        private List<CardStat> cardStats = new List<CardStat>();
        private CardStat newStat = new CardStat();
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

            //selectedCard = _placeHolderBlank;
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

            dropDownContent = new GUIContent(cardType);
            if (EditorGUILayout.DropdownButton(dropDownContent, FocusType.Keyboard, GUILayout.Width(FIELD_WIDTH)))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string type in CARD_TYPES)
                {
                    menu.AddItem(new GUIContent(type), cardType == type,() => SelectCardType(type));
                }
                menu.ShowAsContext();
            }
            cardName = EditorGUILayout.TextField("Card Name", cardName, GUILayout.Width(FIELD_WIDTH));
            artwork = (Texture2D)EditorGUILayout.ObjectField("Artwork", artwork, typeof(Texture2D), false,
                GUILayout.Height(200), GUILayout.Width(FIELD_WIDTH));
            
            
            // start creating new elements
            newStat.statName = EditorGUILayout.TextField("Stat Name", newStat.statName, GUILayout.Width(FIELD_WIDTH));
            newStat.statValue = EditorGUILayout.IntField("Stat Value", newStat.statValue,GUILayout.Width(200));
            newStat.statDescription = EditorGUILayout.TextField("Stat Description", newStat.statDescription, GUILayout.Width(FIELD_WIDTH));

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
                    stat.statName = EditorGUILayout.TextField("Name: ", stat.statName, GUILayout.Width(200));
                    EditorGUILayout.EndVertical();
                
                    EditorGUILayout.BeginVertical(GUILayout.Width(100));
                    stat.statValue = EditorGUILayout.IntField("Value: ", stat.statValue,GUILayout.Width(100));
                    EditorGUILayout.EndVertical();
                
                    EditorGUILayout.BeginVertical(GUILayout.Width(200));
                    stat.statDescription = EditorGUILayout.TextArea(stat.statDescription);
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
            }
            
            
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

        private void SelectCardType(string selectedCardType)
        {
            cardType = selectedCardType;
            dropDownContent.text = cardType;
        }
        
        private void InitializeCard(CardSO card)
        {
            card.CardType = cardType;
            card.CardName = cardName;
            card.CardStats = cardStats;
            // Assign other stats and text fields
            card.ArtWork = artwork;
            card.CardText = cardText;
        }

        private void CreateNewCard()
        {
            CardSO newCard = CreateInstance<CardSO>();
            InitializeCard(newCard);
            AssetDatabase.CreateAsset(newCard, $"{ASSET_PATH}{cardName}.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newCard;
            RefreshCardList(true);
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
                cardType = selectedCard.CardType;
                cardName = selectedCard.CardName;
                cardStats = selectedCard.CardStats;
                // Assign other stats and text fields
                artwork = selectedCard.ArtWork;
                cardText += _stringBuilder.Append(selectedCard.CardText);
                _stringBuilder.Clear();;
            }
        }

        private void UnloadCard()
        {
            selectedCard = null;
            cardType = CARD_TYPES[0];
            cardName = string.Empty;
            cardText = string.Empty;
            cardStats = new List<CardStat>();
            artwork = null;
        }

        private void SaveExistingCard()
        {
            if (!ReferenceEquals(selectedCard, null))
            {
                Undo.RecordObject(selectedCard, "Edited Card");
                InitializeCard(selectedCard);
                EditorUtility.SetDirty(selectedCard);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            UnloadCard();
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