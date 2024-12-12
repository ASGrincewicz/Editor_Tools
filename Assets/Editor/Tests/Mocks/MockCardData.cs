using System.Collections.Generic;
using Editor.CardData;
using Editor.CardData.Stats;
using UnityEngine;

namespace Editor.Tests.Mocks
{
    /// <summary>
    /// This class represents a card configuration without using ScriptableObject.
    /// </summary>
    public class MockCardData // Changed to regular C# class
    {
        private string _cardSetName = "None";
        private int _cardNumber = 0;
        private CardRarity _rarity = CardRarity.None;
        private int _cost;
        private MockCardTypeData _mockCardTypeData;
        private string _cardName;
        private Texture2D _artWork;
        private Keyword[] _keywords;
        private string _cardText;
        private List<CardStat> _stats;
        
        public string CardSetName
        {
            get { return string.IsNullOrEmpty(_cardSetName) ? "None" : _cardSetName; }
            set { _cardSetName = value; }
        }

        public int CardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }

        public CardRarity Rarity
        {
            get { return _rarity; }
            set { _rarity = value; }
        }

        public int CardCost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public MockCardTypeData MockCardTypeData
        {
            get { return _mockCardTypeData; }
            set { _mockCardTypeData = value; }
        }

        public string CardName
        {
            get { return _cardName; }
            set { _cardName = value; }
        }

        public Texture2D ArtWork
        {
            get { return _artWork; }
            set { _artWork = value; }
        }

        public Keyword[] Keywords
        {
            get { return _keywords ??= new Keyword[3]; }
            set { _keywords = value; }
        }

        public string CardText
        {
            get { return _cardText; }
            set { _cardText = value; }
        }

        public List<CardStat> Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }

        private int GetKeywordsTotalValue()
        {
            // Mock implementation (might not load KeywordManager)
            int total = 0;
            foreach (Keyword keyword in _keywords)
            {
                total += keyword.KeywordValue;
            }
            return total;
        }

        public string GetKeywordsSumString()
        {
            if (Keywords == null || Keywords.Length == 0)
            {
                return "No Keywords Assigned to this card.";
            }
            return $"{Keywords[0].KeywordName}({Keywords[0].KeywordValue}) + {Keywords[1].KeywordName}({Keywords[1].KeywordValue}) + {Keywords[2].KeywordName}({Keywords[2].KeywordValue}) = {GetKeywordsTotalValue()}";
        }
    }
}