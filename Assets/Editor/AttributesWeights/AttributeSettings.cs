using System.Collections.Generic;
using UnityEngine;

namespace Editor.AttributesWeights
{
    [CreateAssetMenu(fileName = "AttributeSettings", menuName = "Settings/Attributes", order = 0)]
    public class AttributeSettings : ScriptableObject
    {
        public List<AttributeWeightsData> attributeData;
    }
}