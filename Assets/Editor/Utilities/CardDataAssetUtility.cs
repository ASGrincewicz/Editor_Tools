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
        public static CardStat? AttackStat { get; set; }
        public static int attackValue;
        public static CardStat? ExploreStat { get; set; }
        public static int exploreValue;
        public static CardStat? FocusStat { get; set; }
        public static int focusValue;
        public static CardStat? HitPointsStat { get; set; }
        public static int hitPointsValue;
        public static CardStat? SpeedStat { get; set; }
        public static int speedValue;
        public static CardStat? UpgradeSlotsStat { get; set; }
        public static int upgradeSlotsValue;
        public static string CardText { get; set; }
        public static int CardCost { get; set; }
        public static List<Keyword> KeywordsList { get; set; }
        public static List<string> KeywordNamesList { get; set; }
        public static Keyword[] SelectedKeywords { get; set; }
        public static int[] SelectedKeywordsIndex { get; set; }
        public static CardDataSO SelectedCard { get; set; }
        public static CardDataSO CardToEdit { get; set; }

        public static void CreateNewCard()
        {
            CardDataSO newCard = ScriptableObject.CreateInstance<CardDataSO>();
            if (InitializeCard(newCard))
            {
                AssetDatabase.CreateAsset(newCard, $"{ASSET_PATH}{CardName}.asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newCard;
                CardToEdit = newCard;
                SelectedCard = CardToEdit;
            }
            else
            {
                Debug.LogError($"Failed to create card.");
            }

            Debug.Log("Card Created.");
            CardToEdit = null;
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
            UnloadCard();
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
                }
            }

            if (SelectedCard == null) return;
            LoadCommonCardData();
            UpdateSelectedKeywordsIndices();
            CardText += CardTextStringBuilder.Append(SelectedCard.CardText);
            /*GetStatsFromLoadedCard();*/

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
            CardTypeData = null;
            CardName = string.Empty;
            CardRarity = CardRarity.None;
            CardText = string.Empty;
            AttackStat = null;
            HitPointsStat = null;
            SpeedStat = null;
            FocusStat = null;
            ExploreStat = null;
            UpgradeSlotsStat = null;
            SelectedKeywords = null;
            Artwork = null;
        }

        public static void SaveExistingCard()
        {
            Undo.RecordObject(SelectedCard, "Edited Card");
            if (InitializeCard(SelectedCard))
            {
                //WeightDataAssetUtility.SetWeightData(SelectedCard);
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
      

        private static bool InitializeCard(CardDataSO card)
        {
            if (ReferenceEquals(card, null))
            {
                return false;
            }

            InitializeCommonCardData(card);
            AssignStatsToCard(card);
            //WeightDataAssetUtility.SetWeightData(card);
            
            return true;
        }

        private static void InitializeCommonCardData(CardDataSO card)
        {
            card.CardTypeDataSO = CardTypeData;
            card.Rarity = CardRarity;
            card.CardName = CardName;
            card.ArtWork = Artwork;
            card.Keywords = SelectedKeywords;
            card.CardText = CardText;
            card.CardCost = CardCost;
        }

        private static void AssignStatsToCard(CardDataSO card)
        {
            if (card.CardTypeDataSO != null && card.CardTypeDataSO.HasStats)
            {
                card.Stats ??= new List<CardStat>();
                card.Stats.Add(new CardStat(AttackStat.ToString(), attackValue, ATTACK_DESCRIPTION));
                card.Stats.Add( new CardStat(HitPointsStat.ToString(), hitPointsValue, HIT_POINTS_DESCRIPTION));
                card.Stats.Add(new CardStat(SpeedStat.ToString(), speedValue, SPEED_DESCRIPTION));
                card.Stats.Add(new CardStat(FocusStat.ToString(), focusValue, FOCUS_DESCRIPTION));
                card.Stats.Add( new CardStat(UpgradeSlotsStat.ToString(), upgradeSlotsValue, UPGRADE_SLOTS_DESCRIPTION));
            }
        }
    }
}