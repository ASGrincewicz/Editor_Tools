using Editor.AttributesWeights;
using UnityEngine;

namespace Editor.CostCalculator
{
    [CreateAssetMenu(fileName = "CostCalcSettings", menuName = "Settings/Cost Calculator", order = 1)]
    public class CostCalculatorSettings : ScriptableObject
    {
        public AttributeSettings attributeSettings;

        public float baseNormalizationRate;
        public float secondTierNormalizationRate;
        public float thirdTierNormalizationRate;

        public int normalizationMaximum;
        public int normalizationMinimum;




    }
}