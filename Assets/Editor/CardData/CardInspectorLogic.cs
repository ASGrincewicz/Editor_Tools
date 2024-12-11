namespace Editor.CardData
{
    public class CardInspectorLogic
    {
        public (string,string) FormatPropertyLabel(string propertyName, object value)
        {
            return ($"{propertyName}", $"{value}");
        }
        public bool ShouldDrawCardStatData(bool hasStats)
        {
            return hasStats;
        }
        public bool ShouldDrawKeywordData(bool hasKeywords)
        {
            return hasKeywords;
        }
        public bool ShouldDrawCardTextData(bool hasCardText)
        {
            return hasCardText;
        }
        public bool ShouldDrawCostData(bool hasCost)
        {
            return hasCost;
        }
    }
}