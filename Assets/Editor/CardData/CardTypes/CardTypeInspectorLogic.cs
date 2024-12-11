namespace Editor.CardData.CardTypes
{
    public class CardTypeInspectorLogic
    {
        public (string,string) FormatPropertyLabel(string propertyName, object value)
        {
            return ($"{propertyName}", $"{value}");
        }

        public bool ShouldDrawCardStatData(bool hasStats)
        {
            return hasStats;
        }
    }
}