using System;
using UnityEngine;

namespace Editor.KeywordSystem
{
    [System.Serializable]
    public struct Keyword : IEquatable<Keyword>
    {
        public string keywordName;
        public int keywordValue;
        [Multiline]
        public string definition;
        public AbilityType abilityType;
        

        public Keyword(AbilityType abilityType, string keywordName, string definition,int keywordValue)
        {
            this.abilityType = abilityType;
            this.keywordName = keywordName;
            this.keywordValue = keywordValue;
            this.definition = definition;
        }

        public bool Equals(Keyword other)
        {
            return keywordName == other.keywordName && keywordValue == other.keywordValue && definition == other.definition && abilityType == other.abilityType;
        }

        public override bool Equals(object obj)
        {
            return obj is Keyword other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(keywordName, keywordValue, definition, (int)abilityType);
        }
        
        public bool IsDefault()
        {
            return string.IsNullOrEmpty(keywordName) && keywordValue == 0 && string.IsNullOrEmpty(definition) && abilityType == 0;
        }
    }
}