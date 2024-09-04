using System;
using Editor.AttributesWeights;
using Editor.CardEditor;
using Editor.KeywordSystem;
using UnityEngine;

namespace Editor.CostCalculator
{
    public class CostCalculator
    {
        private const float InitialTotalCost = 0.0f;
        // weight container
        private WeightContainer _weightContainer;
        // card stats
        private CardStat[] _cardStats;
        private Keyword[] _keywords;

        public CostCalculator(WeightContainer weightContainer, CardStat[] cardStats, Keyword[] keywords)
        {
            _weightContainer = weightContainer;
            _cardStats = cardStats;
            _keywords = keywords;
        }
        
        // multiply stat values by corresponding weights
        private float CalculateCost()
        {
            float totalCost = InitialTotalCost;

            if (_weightContainer.weightType == WeightType.Keyword)
            {
                totalCost += AddKeywordValues(_weightContainer.cardStatWeights[0].statWeight);
            }
            else
            {
                totalCost = AddCardStats(totalCost, _weightContainer.cardStatWeights.Length);
                Debug.Log($"Weights Size:{_weightContainer.cardStatWeights.Length}'");
            }
            Debug.Log($"Pre-normalized cost is: {totalCost}");
            return Mathf.Ceil(totalCost);
        }

        private float AddCardStats(float totalCost, int count)
        {
            for (int i = 0; i <= count - 2; i++)
            {
                if (_cardStats[i].StatValue == 0 && i + 1 < count)
                {
                    _weightContainer.cardStatWeights[i + 1].statWeight += _weightContainer.cardStatWeights[i].statWeight;
                }
                else
                {
                    totalCost += _weightContainer.cardStatWeights[i].statWeight * _cardStats[i].StatValue;
                }
            }
            totalCost += AddKeywordValues(_weightContainer.cardStatWeights[count - 1].statWeight);
            return totalCost;
        }

        private int AddKeywordValues(float keywordWeight)
        {
            int keywordCost = 0;
            for (int i = 0; i < _keywords.Length; i++)
            {
                keywordCost += _keywords[i].keywordValue;
            }
            return (int)(keywordCost * keywordWeight);
        }
       
        public int NormalizeCost()
        {
            float normal = 3.0f;
            float cardWeight = CalculateCost();
            if (cardWeight is <= 65.0f and >= 45.0f)
            {
                normal = 2.0f;
            }
            else if (cardWeight is < 45.0f and >= 25.0f)
            {
                normal = 2.5f;
            }
            
            float normalizedCost = cardWeight / normal;
    
            // Clamp the value between 1 and 12
            normalizedCost = Math.Max(1, normalizedCost);
            normalizedCost = Math.Min(12, normalizedCost);

            return (int)normalizedCost;
        }
    }
}