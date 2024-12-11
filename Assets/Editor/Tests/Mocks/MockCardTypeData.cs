using System.Collections.Generic;
using Editor.CardData.Stats;
using UnityEngine;

namespace Editor.Tests.Mocks
{
    public class MockCardTypeData
    {
        private string _cardTypeName;
        private Texture2D _cardTypeIcon;
        private Color _cardTypeColor;
        private bool _hasStats = true;
        private bool _hasCost = true;
        private bool _hasKeywords = true;
        private bool _hasCardText = true;
        private List<CardStatData> _cardStatData;

        public string CardTypeName
        {
            get { return _cardTypeName; }
            set { _cardTypeName = value; }
        }

        public Texture2D CardTypeIcon
        {
            get { return _cardTypeIcon; }
            set { _cardTypeIcon = value; }
        }

        public Color CardTypeColor
        {
            get { return _cardTypeColor; }
            set { _cardTypeColor = value; }
        }

        public bool HasStats
        {
            get { return _hasStats; }
            set { _hasStats = value; }
        }

        public bool HasCost
        {
            get { return _hasCost; }
            set { _hasCost = value; }
        }

        public bool HasKeywords
        {
            get { return _hasKeywords; }
            set { _hasKeywords = value; }
        }

        public bool HasCardText
        {
            get { return _hasCardText; }
            set { _hasCardText = value; }
        }

        public List<CardStatData> CardStats
        {
            get { return _cardStatData; }
            set { _cardStatData = value; }
        }
    }
}