using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.CardData;
using Editor.CardData.CardTypes;
using Editor.CardData.Stats;
using Editor.KeywordSystem;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Editor.Utilities
{
    public static class CardDataAssetUtility
{
    private const string AssetPath = "Assets/Resources/Scriptable Objects/Cards/";
    private const string KeywordManagerPath = "Assets/Resources/Scriptable Objects/Keywords/KeywordManager.asset";

    public static KeywordManager KeywordManager { get; set; }
    public static string[] CardAssetGuids { get; } = LoadAllCardDataByGuid();
    public static List<CardDataSO> AllCardData { get; } = LoadAllCardData();
    public static StringBuilder CardTextStringBuilder { get; set; }
    public static CardTypeDataSO CardTypeData { get; set; }
    public static CardRarity CardRarity { get; set; }
    public static string CardName { get; set; }
    [CanBeNull] public static Texture2D Artwork { get; set; }
    public static string CardText { get; set; }
    public static int CardCost { get; set; }
    public static List<Keyword> KeywordsList { get; set; } = new();
    public static List<string> KeywordNamesList { get; set; } = new();
    public static Keyword[] SelectedKeywords { get; set; }
    public static int[] SelectedKeywordsIndex { get; set; }
    public static List<CardStat> CardStats { get; set; } = new();
    public static CardDataSO CardToEdit { get; set; }
    public static bool StatsLoaded { get; set; }

    public static void UpdateStats(List<CardStat> stats)
    {
        CardStats = stats;
    }

    public static CardTypeDataSO LoadCardTypeData(CardTypeDataSO cardTypeData)
    {
        InitializeCardToEditIfNull();
        if (CardToEdit != null)
        {
            CardTypeData = cardTypeData;
            //AssignStatsToCard();
        }
        return CardTypeData;
    }

    public static void CreateNewCard(List<CardStat> newStats)
    {
        InitializeCardToEditIfNull();
        if (InitializeCard(CardToEdit, newStats))
        {
            SaveCardAsset();
            FocusOnNewCard();
            Debug.Log("Card Created.");
        }
        else
        {
            Debug.LogError("Failed to create card.");
        }
    }

    public static void LoadKeywordManagerAsset()
    {
        KeywordManager = AssetDatabase.LoadAssetAtPath<KeywordManager>(KeywordManagerPath);
    }

    public static void RefreshKeywordsList()
    {
        KeywordsList = KeywordManager?.keywordList?.Where(keyword => !keyword.IsDefault()).ToList() ?? new();
        KeywordNamesList = KeywordsList.Select(keyword => keyword.keywordName).ToList();
    }

    public static void LoadCardFromFile()
    {
        InitializeCardToEditIfNull();
        if (CardToEdit != null)
        {
            CardTypeData = LoadCardTypeData(CardToEdit.CardTypeDataSO);
            LoadCommonCardData();
            UpdateSelectedKeywordsIndices();
            LoadCardTextAndStats();
        }
    }

    public static CardDataSO LoadCardDataByGuid(string guid)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        return AssetDatabase.LoadAssetAtPath<CardDataSO>(path);
    }

    public static void UnloadCard()
    {
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
        return CardAssetGuids.Select(LoadCardDataByGuid).Where(cardData => cardData != null).ToList();
    }

    private static string[] LoadAllCardDataByGuid()
    {
        return AssetDatabase.FindAssets("t:CardDataSO");
    }

    private static void LoadCommonCardData()
    {
        if (CardToEdit != null)
        {
            CardTypeData = CardToEdit.CardTypeDataSO;
            CardName = CardToEdit.CardName;
            CardRarity = CardToEdit.Rarity;
            Artwork = CardToEdit.ArtWork;
            CardCost = CardToEdit.CardCost;
        }
    }

    private static void UpdateSelectedKeywordsIndices()
    {
        SelectedKeywords = CardToEdit?.Keywords;
        SelectedKeywordsIndex = SelectedKeywords?.Select(keyword => KeywordNamesList.IndexOf(keyword.keywordName)).ToArray();
    }

    private static void InitializeCardToEditIfNull()
    {
        if (CardToEdit == null)
        {
            CardToEdit = ScriptableObject.CreateInstance<CardDataSO>();
            Debug.LogWarning("CardToEdit was null and has been initialized.");
        }
    }

    private static void SaveCardAsset()
    {
        AssetDatabase.CreateAsset(CardToEdit, $"{AssetPath}{CardName}.asset");
        AssetDatabase.SaveAssets();
    }

    private static void FocusOnNewCard()
    {
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = CardToEdit;
    }

    private static void LoadCardTextAndStats()
    {
        CardText = CardToEdit.CardText;
        CardStats = CardToEdit.Stats?.ToList() ?? InitializeDefaultCardStats();
        CardTextStringBuilder?.Clear();
    }

    private static List<CardStat> InitializeDefaultCardStats()
    {
        return CardTypeData?.CardStats.Select(stat => new CardStat(stat.statName, 0, stat.statDescription)).ToList() ?? new();
    }

    private static bool InitializeCard(CardDataSO card, List<CardStat> stats)
    {
        if (ReferenceEquals(card, null)) return false;

        InitializeCommonCardData(card);
        AssignStatsToCard(card, stats);
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

    private static void AssignStatsToCard(CardDataSO card, List<CardStat> stats)
    {
        if (card.CardTypeDataSO?.HasStats == true)
        {
            card.Stats = stats;
        }
    }
}
}