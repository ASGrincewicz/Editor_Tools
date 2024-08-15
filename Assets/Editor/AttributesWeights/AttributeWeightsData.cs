﻿using System.Collections.Generic;
using Editor.CardEditor;

namespace Editor.AttributesWeights
{
    [System.Serializable]
    public struct CardStatWeight
    {
        public CardStat Stat;
        public float Weight;

        public CardStatWeight(CardStat stat, float weight)
        {
            Stat = stat;
            Weight = weight;
        }
    }

    [System.Serializable]
    public struct AttributeWeightsData
    {
        public CardTypes CardType;
        public  Dictionary<CardStat, float> Weights;

        public AttributeWeightsData(CardTypes cardType, IEnumerable<CardStatWeight> weights)
        {
            CardType = cardType;
            Weights = new Dictionary<CardStat, float>();
            
            foreach (CardStatWeight weight in weights)
            {
                Weights[weight.Stat] = weight.Weight;
            }
        }
        
        public AttributeWeightsData(CardTypes cardType, CardStat stat, float weight)
        {
            CardType = cardType;
            Weights = new Dictionary<CardStat, float>
            {
                [stat] = weight
            };
        }
    }
}