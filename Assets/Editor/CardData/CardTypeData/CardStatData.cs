using System;

namespace Editor.CardData.CardTypeData
{
    [Serializable]
    public struct CardStatData
    {
        public string statName;
        public string statDescription;
        public float statWeight;

        public CardStatData(string statName, string statDescription, float statWeight )
        {
            this.statName = statName;
            this.statDescription = statDescription;
            this.statWeight = statWeight;
        }
    }
}