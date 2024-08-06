using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(menuName="Config/CardData")]
    public class CardSO : ScriptableObject, ICardData
    {
        [SerializeField] private string _cardType;
        [SerializeField] private string _cardName;
        [SerializeField] private List<CardStat> _cardStats;
        [SerializeField] private Texture2D _artWork;
        [SerializeField][Multiline]
        private string _cardText;

        public string CardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }

        public string CardName
        {
            get { return _cardName; }
            set { _cardName = value; }
        }

        public List<CardStat> CardStats
        {
            get { return _cardStats; }
            set { _cardStats = value; }
        }

        public Texture2D ArtWork
        {
            get { return _artWork; }
            set { _artWork = value; }
        }

        public string CardText
        {
            get { return _cardText; }
            set { _cardText = value; }
        }
    }
}
