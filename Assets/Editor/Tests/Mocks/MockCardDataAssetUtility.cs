using System.Collections.Generic;
using Editor.CardData;
using Editor.CardData.Stats;
using UnityEngine;

namespace Editor.Tests.Mocks
{
    public class MockCardDataAssetUtility
    {
        public List<CardDataSO> AllCardData { get; set; } = new();
        public MockKeywordManager MockKeywordManager { get; set; }
        public string[] CardAssetGuids { get; set; } = { "1234", "5678" };
        public MockCardTypeData CardTypeData { get; set; }
        public string CardName { get; set; } = "MockCard";
        public Texture2D Artwork { get; set; }
        public string CardText { get; set; } = "This is a mock card text.";
        public int CardCost { get; set; } = 0;
        public List<CardStat> CardStats { get; set; } = new();
        public MockCardData CardToEdit { get; set; }
        public List<Keyword> KeywordsList { get; set; } = new();
        public List<string> KeywordNamesList { get; set; } = new();
        
        public static Keyword[] SelectedKeywords { get; set; }

        public void LoadKeywordManagerAsset()
        {
            // Simulate loading the KeywordManager.
            MockKeywordManager = new MockKeywordManager()
            {
                KeywordList = new List<Keyword>
                {
                    new() { KeywordName = "MockKeyword1", Definition = "Description1" },
                    new() { KeywordName = "MockKeyword2", Definition = "Description2" }
                }
            };
        }

        public void RefreshKeywordsList()
        {
            KeywordsList = MockKeywordManager?.KeywordList ?? new List<Keyword>();
            KeywordNamesList = KeywordsList.ConvertAll(keyword => keyword.KeywordName);
        }

        public void CreateNewCard(List<CardStat> newStats)
        {
            CardToEdit = new MockCardData()
            {
                CardName = CardName,
                MockCardTypeData = CardTypeData,
                ArtWork = Artwork,
                CardText = CardText,
                Keywords = KeywordsList.ToArray(),
                Stats = newStats
            };
        }

        public void SaveExistingCard(List<CardStat> stats)
        {
            // In a mock environment, simply update the mock CardDataSO instance.
            if (CardToEdit != null)
            {
                CardToEdit.CardName = CardName;
                CardToEdit.MockCardTypeData = CardTypeData;
                CardToEdit.ArtWork = Artwork;
                CardToEdit.CardText = CardText;
                CardToEdit.Keywords = KeywordsList.ToArray();
                CardToEdit.Stats = stats;
                CardToEdit.CardCost = CardCost;
            }
        }

        public void UnloadCard()
        {
            // Reset mock fields.
            CardToEdit = null;
            CardTypeData = null;
            CardName = string.Empty;
            CardText = string.Empty;
            Artwork = null;
        }

        public void LoadCardFromFile()
        {
            // Simulate loading a card.
            CardToEdit = new MockCardData()
            {
                CardName = "LoadedMockCard",
                MockCardTypeData = CardTypeData,
                ArtWork = Artwork,
                CardText = CardText,
                Stats = CardStats,
            };
        }

        public void UpdateStats(List<CardStat> newStats)
        {
            CardStats = newStats;
        }

        public MockCardTypeData LoadCardTypeData(MockCardTypeData mockCardTypeData)
        {
           return mockCardTypeData;
        }
    }
}