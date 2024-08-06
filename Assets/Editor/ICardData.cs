using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    public interface ICardData
    {
        public string CardType { get;  set; }
        public string CardName { get; set; }
        public List<CardStat> CardStats { get; set; }
        // Add other stats and text fields
        public Texture2D ArtWork { get; set; }
        public string CardText { get; set; }
    }
}