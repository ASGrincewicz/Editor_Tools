using System.Collections.Generic;
using UnityEngine;

namespace Editor.CardData.CardTypeData
{
    [CreateAssetMenu(fileName = "CardTypeDataSO, menuName = Card Type Data", order = 0)]
    public class CardTypeDataSO : ScriptableObject
    {
        [SerializeField] private string _cardTypeName;
        [SerializeField] private Texture2D _cardTypeIcon;
        [SerializeField] private Color _cardTypeColor;
        [SerializeField] private bool _hasStats = true;
        [SerializeField] private bool _hasCost = true;
        [SerializeField] private bool _hasKeywords = true;
        [SerializeField] private bool _hasCardText = true;
        [SerializeField] private List<CardStatData> _cardStatData;

        public string CardTypeName
        {
            get => _cardTypeName;
            set => _cardTypeName = value;
        }

        public Texture2D CardTypeIcon
        {
            get => _cardTypeIcon;
            set => _cardTypeIcon = value;
        }

        public Color CardTypeColor
        {
            get => _cardTypeColor;
            set => _cardTypeColor = value;
        }

        public bool HasStats
        {
            get => _hasStats;
            set => _hasStats = value;
        }

        public bool HasCost
        {
            get => _hasCost;
            set => _hasCost = value;
        }

        public bool HasKeywords
        {
            get => _hasKeywords;
            set => _hasKeywords = value;
        }

        public bool HasCardText
        {
            get => _hasCardText;
            set => _hasCardText = value;
        }

        public List<CardStatData> CardStats
        {
            get => _cardStatData;
            set => _cardStatData = value;
        }
    }
}