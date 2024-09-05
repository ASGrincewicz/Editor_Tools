using System;
using UnityEngine;

namespace Editor.AttributesWeights
{
    [CreateAssetMenu(fileName = "WeightContainer", menuName = "Config/Weight Container", order = 0)]
    public class WeightContainer : ScriptableObject
    {
        public WeightType weightType;
        public CardStatWeight[] cardStatWeights;

        private const string PLACEHOLDER_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Other/PlaceHolder_Data.asset";
    }
}