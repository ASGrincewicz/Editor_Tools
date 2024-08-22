using Editor.AttributesWeights;
using UnityEngine;

namespace Editor.CardEditor
{
    /// <summary>
    /// This class represents a card configuration as a ScriptableObject.
    /// It implements the ICardData interface and is used to create new card configs in Unity.
    /// </summary>
    [CreateAssetMenu(menuName="Config/CardData")] 
    public class CardSO : ScriptableObject, ICardData
    {
        [HideInInspector] [SerializeField] public WeightContainer _weightData;
        [HideInInspector] [SerializeField] private CardTypes _cardType;
        [HideInInspector] [SerializeField] private string _cardName;
        [HideInInspector] [SerializeField] private Texture2D _artWork;
        // Stats
        [HideInInspector] [SerializeField] private CardStat _attack;
        [HideInInspector] [SerializeField] private CardStat _explore;
        [HideInInspector] [SerializeField] private CardStat _focus;
        [HideInInspector] [SerializeField] private CardStat _hitPoints;
        [HideInInspector] [SerializeField] private CardStat _speed;
        [HideInInspector] [SerializeField] private CardStat _upgradeSlots;
        
        [HideInInspector] [SerializeField][Multiline]
        private string _cardText;

        /// <summary>
        /// Type of the card.
        /// </summary>
        public CardTypes CardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }
        /// <summary>
        /// Name of the card.
        /// </summary>
        public string CardName
        {
            get { return _cardName; }
            set { _cardName = value; }
        }
        
        /// <summary>
        /// Artwork of the card.
        /// </summary>
        public Texture2D ArtWork
        {
            get { return _artWork; }
            set { _artWork = value; }
        }
        /// <summary>
        /// Text description of the card.
        /// </summary>
        public string CardText
        {
            get { return _cardText; }
            set { _cardText = value; }
        }
        
        // The following properties are the CardStat type properties representing different stats for a card
        public CardStat Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public CardStat Explore
        {
            get { return _explore; }
            set { _explore = value; }
        }

        public CardStat Focus
        {
            get { return _focus; }
            set { _focus = value; }
        }

        public CardStat HitPoints
        {
            get { return _hitPoints; }
            set { _hitPoints = value; }
        }

        public CardStat Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public CardStat UpgradeSlots
        {
            get { return _upgradeSlots; }
            set { _upgradeSlots = value; }
        }

        public CardStat[] GetCardStats()
        {
            CardStat[] stats = new CardStat[]
            {
                _attack, _explore, _focus, _hitPoints, _speed, _upgradeSlots
            };
            return stats;
        }
    }
}
