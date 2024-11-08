namespace Editor.CardData
{
    [System.Serializable]
    public class CardStat
    {
        public string statName;
        public int statValue;
        public string statDescription;
      

        public CardStat(string statName, int statValue = 0, string statDescription = "empty")
        {
            this.statName = statName;
            this.statValue = statValue;
            this.statDescription = statDescription;
        }
    }
}