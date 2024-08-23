namespace Editor.KeywordSystem
{
    [System.Serializable]
    public struct Keyword
    {
        public string keywordName;
        public int keywordValue;
        public AbilityType abilityType;
        

        public Keyword(AbilityType abilityType, string keywordName, int keywordValue)
        {
            this.abilityType = abilityType;
            this.keywordName = keywordName;
            this.keywordValue = keywordValue;
        }
    }
}