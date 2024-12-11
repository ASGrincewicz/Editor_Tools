namespace Editor.CardData.Stats
{
    [System.Serializable]
    public class CardStat
    {
        public string statName;
        public int statValue;
        public string statDescription;
      

        public CardStat(string statName = "Stat Name", int statValue = 0, string statDescription = "empty")
        {
            this.statName = statName;
            this.statValue = statValue;
            this.statDescription = statDescription;
        }
    }
}