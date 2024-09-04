using Editor.CardEditor;
using UnityEngine;

namespace Editor.AttributesWeights
{
    [CreateAssetMenu(fileName = "WeightContainer", menuName = "Config/Weight Container", order = 0)]
    public class WeightContainer : ScriptableObject
    {
        public WeightType weightType;
        public CardStatWeight[] cardStatWeights;
    }
}