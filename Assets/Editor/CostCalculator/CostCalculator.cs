using System;
using Editor.CardData;
using Editor.CardData.Stats;
using Editor.KeywordSystem;
using UnityEngine;

namespace Editor.CostCalculator
{
    public class CostCalculator
    {
        private const float InitialTotalCost = 0.0f;
        
        private CostCalculatorSettings CostCalculatorSettings { get; set; }
        private CardStat[] CardStatsArray { get; set; }
        private Keyword[] KeywordsArray { get; set; }
        

        public CostCalculator(CostCalculatorSettings settings,  CardStat[] cardStatsArray, Keyword[] keywordsArray)
        {
            CostCalculatorSettings = settings;
            CardStatsArray = cardStatsArray;
            KeywordsArray = keywordsArray;
        }
       
        public int NormalizeCost()
        {
            float normal = CostCalculatorSettings.baseNormalizationRate;
            float cardWeight = CalculateCost();
            if (cardWeight is <= 65.0f and >= 45.0f)
            {
                normal = CostCalculatorSettings.secondTierNormalizationRate;
            }
            else if (cardWeight is < 45.0f and >= 25.0f)
            {
                normal = CostCalculatorSettings.thirdTierNormalizationRate;
            }
            
            float normalizedCost = cardWeight / normal;
            
            normalizedCost = Math.Max(CostCalculatorSettings.normalizationMinimum, normalizedCost);
            normalizedCost = Math.Min(CostCalculatorSettings.normalizationMaximum, normalizedCost);

            return (int)normalizedCost;
        }
        private float CalculateCost()
        {
            float totalCost = InitialTotalCost;
            
            return Mathf.Ceil(totalCost);
        }
        private float AddCardStats(float totalCost, int count)
        {
            return totalCost;
        }
        
        private int AddKeywordValues(float keywordWeight)
        {
            int keywordCost = 0;
            for (int i = 0; i < KeywordsArray.Length; i++)
            {
                keywordCost += KeywordsArray[i].keywordValue;
            }
            return (int)(keywordCost * keywordWeight);
        }
    }
}