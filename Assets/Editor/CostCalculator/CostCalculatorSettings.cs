using Editor.AttributesWeights;
using UnityEngine;

namespace Editor.CostCalculator
{
    [CreateAssetMenu(fileName = "CostCalcSettings", menuName = "Settings/Cost Calculator", order = 1)]
    public class CostCalculatorSettings : ScriptableObject
    {
        public AttributeSettings attributeSettings;
        
       
       
        
    }
}