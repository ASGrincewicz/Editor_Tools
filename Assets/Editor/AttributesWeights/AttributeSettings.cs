using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editor.AttributesWeights
{
    [CreateAssetMenu(fileName = "AttributeSettings", menuName = "Settings/Attributes", order = 0)]
    public class AttributeSettings : ScriptableObject
    {
        public CardStatWeight[] allyCardStatWeights;
        public CardStatWeight[] bossCardStatWeights;
        public CardStatWeight[] creatureCardStatWeights;
        public CardStatWeight[] hunterCardStatWeights;

        public CardStatWeight[] environmentCardStatWeights;
        
        [Tooltip("Keyword Only Card Types: Action, Gear")]
        public CardStatWeight[] keywordOnlyCardStatWeights;
    }
}