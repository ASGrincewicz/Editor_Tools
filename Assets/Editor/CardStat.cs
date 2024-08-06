using JetBrains.Annotations;

namespace Editor
{
    [System.Serializable]
    public struct CardStat
    {
        public string statName;
        public int statValue;
        [CanBeNull] public string statDescription;

        public CardStat(string statName, int statValue = 0, string statDescription = null)
        {
            this.statName = statName;
            this.statValue = statValue;
            this.statDescription = statDescription;
        }
    }
}