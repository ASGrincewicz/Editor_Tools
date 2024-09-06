using UnityEngine;

namespace Editor.CardData
{
    public interface ICardData
    {
        public CardTypes CardType { get;  set; }
        public string CardName { get; set; }
        // Add other stats and text fields
        public Texture2D ArtWork { get; set; }
        public string CardText { get; set; }
        public CardRarity Rarity { get; set; }

        public CardStat Attack { get; set; }
        public CardStat Explore{ get; set; }
        public CardStat Focus{ get; set; }
        public CardStat HitPoints{ get; set; }
        public CardStat Speed{ get; set; }
        public CardStat UpgradeSlots{ get; set; }
    }
}