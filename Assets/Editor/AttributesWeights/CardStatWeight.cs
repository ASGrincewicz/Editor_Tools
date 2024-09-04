using Editor.CardEditor;
using UnityEngine;

namespace Editor.AttributesWeights
{
    [CreateAssetMenu(menuName="Config/CardStatWeightData")]
    public class CardStatWeight: ScriptableObject
    {
        //public CardTypes cardType;
        public StatNames statName;
        [Range(0.00f, 1.0f)]
        public float statWeight;
        
    }
}