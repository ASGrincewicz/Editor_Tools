using System;
using Editor.AttributesWeights;
using Editor.CardData;
using Editor.KeywordSystem;
using UnityEngine;

namespace Editor.CostCalculator
{
    public class CostCalculator
    {
        private const float InitialTotalCost = 0.0f;
        
        private CostCalculatorSettings CostCalculatorSettings { get; set; }
        private WeightContainer WeightContainer { get; set; }
        private CardStat[] CardStatsArray { get; set; }
        private Keyword[] KeywordsArray { get; set; }
        

        public CostCalculator(CostCalculatorSettings settings, WeightContainer weightContainer, CardStat[] cardStatsArray, Keyword[] keywordsArray)
        {
            CostCalculatorSettings = settings;
            WeightContainer = weightContainer;
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

            if (WeightContainer.weightType == WeightType.Keyword)
            {
                totalCost += AddKeywordValues(WeightContainer.cardStatWeights[6].statWeight);
            }
            else
            {
                totalCost = AddCardStats(totalCost, WeightContainer.cardStatWeights.Length);
                Debug.Log($"Weights Size:{WeightContainer.cardStatWeights.Length}'");
            }
            Debug.Log($"Pre-normalized cost is: {totalCost}");
            return Mathf.Ceil(totalCost);
        }
        private float AddCardStats(float totalCost, int count)
        {
            for (int i = 0; i <= count - 2; i++)
            {
                totalCost += WeightContainer.cardStatWeights[i].statWeight * CardStatsArray[i].StatValue;
               
            }
            totalCost += AddKeywordValues(WeightContainer.cardStatWeights[count - 1].statWeight);
            Debug.Log($"Total cost after adding keyword weights: {totalCost}");
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