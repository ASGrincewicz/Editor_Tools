using System;
using System.Collections.Generic;
using System.Text;
using Editor.CardData;
using Editor.CardData.CardTypeData;
using Editor.KeywordSystem;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static Editor.CardData.StatDataReference;

namespace Editor.Utilities
{
    public static class CardDataAssetUtility
    {
        private const string ASSET_PATH = "Assets/Data/Scriptable Objects/Cards/";
        private const string KEYWORD_MANAGER_PATH = "Assets/Data/Scriptable Objects/Keywords/KeywordManager.asset";

        public static KeywordManager keywordManager;
        public static string[] CardAssetGUIDs { get; } = LoadAllCardDataByGUID();
        public static List<CardDataSO> AllCardData { get; } = LoadAllCardData();
        public static StringBuilder CardTextStringBuilder { get; set; }
        public static CardTypeDataSO CardTypeData { get; set; }
        public static CardRarity CardRarity { get; set; }
        public static string CardName { get; set; }
        [CanBeNull] public static Texture2D Artwork { get; set; }
        public static string CardText { get; set; }
        public static int CardCost { get; set; }
        
        // Keyword
        public static List<Keyword> KeywordsList { get; set; }
        public static List<string> KeywordNamesList { get; set; }
        public static Keyword[] SelectedKeywords { get; set; }
        public static int[] SelectedKeywordsIndex { get; set; }
        
        // Stats
        public static List<CardStat> CardStats { get; set; }
        //public static List<int> CardStatValues { get; set; }
        
        public static CardDataSO SelectedCard { get; set; }
        public static CardDataSO CardToEdit { get; set; }
        
        public static bool StatsLoaded { get; set; }

        public static void UpdateStats(List<CardStat> stats)
        {
            CardStats??= new List<CardStat>();
            CardStats = stats;
        }

        public static void LoadCardTypeData()
        {
            if (!StatsLoaded)
            {
                CardStats = new List<CardStat>();
            }
            CardStats ??= new List<CardStat>();
            if (CardTypeData == null)
            {
                return;
            }
            List<CardStat> newStats = new List<CardStat>();
            foreach (CardStatData statData in CardTypeData.CardStats)
            {
                if (SelectedCard != null)
                {
                   CardStats = SelectedCard.Stats;
                   Debug.Log($"Selected Card: {SelectedCard.CardName}");
                }
                else
                {
                    newStats.Add(new CardStat(statData.statName,0,statData.statDescription));
                    Debug.Log($"Added Stat:{statData.statName}");
                }
            }
            CardStats = newStats;
            StatsLoaded = true;
            Debug.Log($"Loaded Stats");
        }

        public static void CreateNewCard(List<CardStat> newStats)
        {
            CardToEdit ??= ScriptableObject.CreateInstance<CardDataSO>();
            if (InitializeCard(CardToEdit, newStats))
            {
                AssetDatabase.CreateAsset(CardToEdit, $"{ASSET_PATH}{CardName}.asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = CardToEdit;
                SelectedCard = CardToEdit;
            }
            else
            {
                Debug.LogError($"Failed to create card.");
            }

            Debug.Log("Card Created.");
            CardToEdit = null;
            StatsLoaded = false;
        }

        public static void LoadKeywordManagerAsset()
        {
            keywordManager = AssetDatabase.LoadAssetAtPath<KeywordManager>(KEYWORD_MANAGER_PATH);
        }

        public static void RefreshKeywordsList()
        {
            KeywordsList = new List<Keyword>();
            foreach (Keyword keyword in keywordManager.keywordList)
            {
                if (!keyword.IsDefault())
                {
                    KeywordsList.Add(keyword);
                }
            }

            KeywordNamesList = new List<string>();
            foreach (Keyword keyword in KeywordsList)
            {
                if (!keyword.IsDefault())
                {
                    KeywordNamesList.Add(keyword.keywordName);
                }
            }
        }

        public static void LoadCardFromFile()
        {
            //UnloadCard();
            if (CardToEdit != null)
            {
                SelectedCard = CardToEdit;
            }
            else
            {
                string path = EditorUtility.OpenFilePanel("Select Card", ASSET_PATH, "asset");
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                    CardToEdit = AssetDatabase.LoadAssetAtPath<CardDataSO>(path);
                    SelectedCard = CardToEdit;
                    LoadCardTypeData();
                }
            }

            if (SelectedCard == null) return;
            LoadCommonCardData();
            UpdateSelectedKeywordsIndices();
            CardText = SelectedCard.CardText;
            CardStats = new List<CardStat>();
            foreach (CardStat stat in SelectedCard.Stats)
            {
                CardStats.Add(stat);
            }

            CardTextStringBuilder.Clear();
        }

        public static CardDataSO LoadCardDataByGuid(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<CardDataSO>(path);
        }

        public static void UnloadCard()
        {
            if (SelectedCard == null) return;
            SelectedCard = null;
            CardToEdit = null;
            CardTypeData = null;
            CardName = string.Empty;
            CardRarity = CardRarity.None;
            CardText = string.Empty;
            SelectedKeywords = null;
            Artwork = null;
        }

        public static void SaveExistingCard(List<CardStat> stats)
        {
            Undo.RecordObject(SelectedCard, "Edited Card");
            if (InitializeCard(SelectedCard, stats))
            {
                EditorUtility.SetDirty(SelectedCard);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static List<CardDataSO> LoadAllCardData()
        {
            List<CardDataSO> cardDataList = new();
            foreach (string guid in CardAssetGUIDs)
            {
                CardDataSO cardData = LoadCardDataByGuid(guid);
                if (cardData != null)
                {
                    cardDataList.Add(cardData);
                }
            }
            return cardDataList;
        }

        private static string[] LoadAllCardDataByGUID()
        {
            return AssetDatabase.FindAssets("t:CardDataSO");
        }

        private static void LoadCommonCardData()
        {
            CardTypeData = SelectedCard.CardTypeDataSO;
            LoadCardTypeData();
            CardName = SelectedCard.CardName;
            CardRarity = SelectedCard.Rarity;
            Artwork = SelectedCard.ArtWork;
            CardCost = SelectedCard.CardCost;
        }

        private static void UpdateSelectedKeywordsIndices()
        {
            if (SelectedCard?.Keywords == null)
            {
                Debug.LogError("Selected card or its keywords are null");
                return;
            }

            SelectedKeywords = SelectedCard.Keywords;

            // Ensure SelectedKeywordsIndex has the correct size
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
      

        private static bool InitializeCard(CardDataSO card, List<CardStat> stats)
        {
            if (ReferenceEquals(card, null))
            {
                return false;
            }

            InitializeCommonCardData(card);
            AssignStatsToCard(card, stats);
            
            return true;
        }

        private static void InitializeCommonCardData(CardDataSO card)
        {
            card.CardTypeDataSO = CardTypeData;
            Debug.Log($"Initializing card data for {card.CardTypeDataSO.name}");
            card.Rarity = CardRarity;
            card.CardName = CardName;
            card.ArtWork = Artwork;
            card.Keywords = SelectedKeywords;
            card.CardText = CardText;
            card.CardCost = CardCost;
        }

        private static void AssignStatsToCard(CardDataSO card, List<CardStat> stats)
        {
            if (card.CardTypeDataSO == null)
            {
                Debug.LogError("CardTypeDataSO is null.");
                return;
            }

            if (!card.CardTypeDataSO.HasStats)
            {
                Debug.LogError("CardTypeDataSO does not have stats.");
                return;
            }

            if (card.CardTypeDataSO.CardStats == null || card.CardTypeDataSO.CardStats.Count == 0)
            {
                Debug.LogError("CardTypeDataSO.CardStats is null or empty.");
                return;
            }

            if (stats == null || stats.Count < card.CardTypeDataSO.CardStats.Count)
            {
                Debug.LogError("stats is null or does not contain enough elements.");
                return;
            }

            card.Stats = CardStats;

            /*for (int i = 0; i < CardStats.Count; i++)
            {
                Debug.Log($"Adding stat: {stats[i].StatName} with value: {stats[i].StatValue}");
                card.Stats[i] = new CardStat(
                    stats[i].StatName, 
                    stats[i].StatValue, 
                    stats[i].StatDescription);
            }*/
        }
    }
}