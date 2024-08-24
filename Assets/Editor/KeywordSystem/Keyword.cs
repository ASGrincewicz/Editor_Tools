using UnityEngine;

namespace Editor.KeywordSystem
{
    [System.Serializable]
    public struct Keyword
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
    }
}