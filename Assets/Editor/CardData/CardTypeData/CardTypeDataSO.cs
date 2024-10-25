using System.Collections.Generic;
using Editor.KeywordSystem;
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
        [SerializeField] private List<CardStat> _cardStats;
    }
}