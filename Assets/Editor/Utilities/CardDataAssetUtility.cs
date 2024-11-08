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
        
        public static CardDataSO CardToEdit { get; set; }
        
        public static bool StatsLoaded { get; set; }

        public static void UpdateStats(List<CardStat> stats)
        {
            CardStats??= new List<CardStat>();
            CardStats = stats;
        }

        public static CardTypeDataSO LoadCardTypeData(CardTypeDataSO cardTypeData)
        {
            Debug.Log("Load Card Type Data");
            if (CardToEdit == null)
            {
                CardToEdit ??= ScriptableObject.CreateInstance<CardDataSO>();
                Debug.LogWarning("CardToEdit == null");
            }
            else
            {
                CardTypeData = cardTypeData;
                Debug.Log($"{CardTypeData.CardTypeName}");
                CardStats= new List<CardStat>();
                foreach (CardStatData cardStatData in CardTypeData.CardStats)
                {
                    CardStats.Add(new CardStat(cardStatData.statName, 0, cardStatData.statDescription));
                }
            }
            return CardTypeData;
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
            }
            else
            {
                Debug.LogError($"Failed to create card.");
            }

            Debug.Log("Card Created.");
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
            if (CardToEdit == null)
            {
                string path = EditorUtility.OpenFilePanel("Select Card", ASSET_PATH, "asset");
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                    CardToEdit = AssetDatabase.LoadAssetAtPath<CardDataSO>(path);
                    CardTypeData = LoadCardTypeData(CardToEdit.CardTypeDataSO);
                }
            }
            LoadCommonCardData();
            UpdateSelectedKeywordsIndices();
            CardText = CardToEdit.CardText;
            CardStats = new List<CardStat>();
            foreach (CardStat stat in CardToEdit.Stats)
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
            if (CardToEdit == null) return;
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
            Undo.RecordObject(CardToEdit, "Edited Card");
            if (InitializeCard(CardToEdit, stats))
            {
                EditorUtility.SetDirty(CardToEdit);
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
            CardTypeData = LoadCardTypeData(CardToEdit.CardTypeDataSO);
            CardName = CardToEdit.CardName;
            CardRarity = CardToEdit.Rarity;
            Artwork = CardToEdit.ArtWork;
            CardCost = CardToEdit.CardCost;
        }

        private static void UpdateSelectedKeywordsIndices()
        {
            if (CardToEdit?.Keywords == null)
            {
                Debug.LogError("Selected card or its keywords are null");
                return;
            }

            SelectedKeywords = CardToEdit.Keywords;

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
            Debug.Log($"Initializing card data for {card.CardName}");
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
        }
    }
}