using System;

namespace Editor.CardData.Stats
{
    [Serializable]
    public class CardStatData
    {
        public string statName;
        public string statDescription;
        public float statWeight;

        public CardStatData(string statName = "Stat Name", string statDescription = "Description", float statWeight = 0.0f )
        {
            this.statName = statName;
            this.statDescription = statDescription;
            this.statWeight = statWeight;
        }
    }
}