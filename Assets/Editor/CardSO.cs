using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(menuName="Config/CardData")]
    public class CardSO : ScriptableObject, ICardData
    {
        [SerializeField] private CardTypes _cardType;
        [SerializeField] private string _cardName;
        [SerializeField] private Texture2D _artWork;
        [SerializeField] private CardStat _attack;
        [SerializeField] private CardStat _explore;
        [SerializeField] private CardStat _focus;
        [SerializeField] private CardStat _hitPoints;
        [SerializeField] private CardStat _speed;
        [SerializeField] private CardStat _upgradeSlots;
        
        [SerializeField][Multiline]
        private string _cardText;

        public CardTypes CardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }

        public string CardName
        {
            get { return _cardName; }
            set { _cardName = value; }
        }

        public CardStat Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = value;
            }
        }

        public CardStat Explore
        {
            get
            {
                return _explore;
            }
            set
            {
                _explore = value;
            }
        }

        public CardStat Focus
        {
            get
            {
                return _focus;
            }
            set
            {
                _focus = value;
            }
        }

        public CardStat HitPoints
        {
            get
            {
                return _hitPoints;
            }
            set
            {
                _hitPoints = value;
            }
        }

        public CardStat Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public CardStat UpgradeSlots
        {
            get
            {
                return _upgradeSlots;
            }
            set
            {
                _upgradeSlots = value;
            }
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
