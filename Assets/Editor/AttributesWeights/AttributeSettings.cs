using UnityEngine;

namespace Editor.AttributesWeights
{
    [CreateAssetMenu(fileName = "AttributeSettings", menuName = "Settings/Attributes", order = 0)]
    public class AttributeSettings : ScriptableObject
    {
        public WeightContainer allyCardStatWeights;
        public WeightContainer bossCardStatWeights;
        public WeightContainer creatureCardStatWeights;
        public WeightContainer gearCardStatWeights;
        public WeightContainer hunterCardStatWeights;
        public WeightContainer environmentCardStatWeights;
        
        [Tooltip("Keyword Only Card Types: Action")]
        public WeightContainer keywordOnlyCardStatWeights;
    }
}