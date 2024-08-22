using System;
using Editor.AttributesWeights;
using Editor.CardEditor;
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

        public CostCalculator(WeightContainer weightContainer, CardStat[] cardStats)
        {
            _weightContainer = weightContainer;
            _cardStats = cardStats;
        }
        
        // multiply stat values by corresponding weights
        private float CalculateCost()
        {
            float totalCost = InitialTotalCost;

            switch (_weightContainer.weightType)
            {
                case WeightType.Ally:
                case WeightType.Boss:
                case WeightType.Creature:
                case WeightType.Hunter:
                    if (_weightContainer.weightType == WeightType.Hunter)
                    {
                        totalCost = AddCardStats(totalCost, 5);
                    }
                    else
                    {
                        totalCost = AddCardStats(totalCost, 4);
                    }
                    break;
                case WeightType.Environment:
                    totalCost = AddCardStats(totalCost, 1);
                    break;
                case WeightType.Keyword:
                    totalCost += _weightContainer.cardStatWeights[0].statWeight * _cardStats[0].StatValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Log($"Pre-normalized cost is: {totalCost += SimulateKeywords()}");
            return totalCost;
        }

        private float AddCardStats(float totalCost, int count)
        {
            for (int i = 0; i <= count; i++)
            {
                totalCost += _weightContainer.cardStatWeights[i].statWeight * _cardStats[i].StatValue;
            }
            return totalCost;
        }
        
        // Placeholder for adding Keyword Values
        private int SimulateKeywords()
        {
            // Define the range
            int minRange = 0;
            int maxRange = 25;
    
            // Generate a random float within the specified range
            int simulatedValue = UnityEngine.Random.Range(minRange, maxRange);
            Debug.Log($"Simulated Keyword value: {simulatedValue}");

            return simulatedValue;
        }
        // normalize based on overall weight
        public int NormalizeCost()
        {
            float normal = 3.5f;
            float cardWeight = CalculateCost();
            if (cardWeight is <= 65.0f and >= 45.0f)
            {
                normal = 2.0f;
            }
            else if (cardWeight is < 45.0f and >= 25.0f)
            {
                normal = 3.0f;
            }
            
            float normalizedCost = cardWeight / normal;
    
            // Clamp the value between 1 and 12
            normalizedCost = Math.Max(1, normalizedCost);
            normalizedCost = Math.Min(12, normalizedCost);

            return (int)normalizedCost;
        }
    }
}