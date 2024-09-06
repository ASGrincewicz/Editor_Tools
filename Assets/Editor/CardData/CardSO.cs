using Editor.AttributesWeights;
using Editor.KeywordSystem;
using UnityEditor;
using UnityEngine;

namespace Editor.CardData
{
    /// <summary>
    /// This class represents a card configuration as a ScriptableObject.
    /// It implements the ICardData interface and is used to create new card configs in Unity.
    /// </summary>
    [CreateAssetMenu(menuName="Config/CardData")] 
    public class CardSO : ScriptableObject, ICardData
    {
        private const string KeywordManagerPath = "Assets/Data/Scriptable Objects/Keywords/KeywordManager.asset";
        [HideInInspector] [SerializeField] private WeightContainer _weightData;
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


        [HideInInspector] [SerializeField] private Keyword[] _keywords;
        
        [HideInInspector] [SerializeField][Multiline]
        private string _cardText;

        [HideInInspector] [SerializeField] private int _cost;
        [HideInInspector] [SerializeField] private CardRarity _rarity;

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
        ///  Array of Keywords
        /// </summary>
        public Keyword[] Keywords
        {
            get
            {
                if (_keywords == null)
                {
                    _keywords = new Keyword[3];
                }
                return _keywords;
            }
            set { _keywords = value; }
        }
        /// <summary>
        /// Text description of the card.
        /// </summary>
        public string CardText
        {
            get { return _cardText; }
            set { _cardText = value; }
        }

        public int CardCost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public CardRarity Rarity
        {
            get { return _rarity; }
            set { _rarity = value; }
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
            CardStat[] stats = new[]
            {
                _attack, _explore, _focus, _hitPoints, _speed, _upgradeSlots
            };
            return stats;
        }

        private int GetKeywordsTotalValue()
        {
            GetCurrentKeywordInfo();
            int total = 0;
            foreach (Keyword keyword in _keywords)
            {
                total += keyword.keywordValue;
            }
            return total;
        }

        public string GetKeywordsSumString()
        {
            if (Keywords == null || Keywords.Length == 0)
            {
                return "No Keywords Assigned to this card.";
            }
            return $"{Keywords[0].keywordName}({Keywords[0].keywordValue}) + {Keywords[1].keywordName}({Keywords[1].keywordValue}) + {Keywords[2].keywordName}({Keywords[2].keywordValue}) = {GetKeywordsTotalValue()}";
        }

        private void GetCurrentKeywordInfo()
        {
            KeywordManager manager = LoadKeywordManager();
            if (manager != null)
            {
                UpdateKeywords(manager);
            }
        }

        private KeywordManager LoadKeywordManager()
        {
            return AssetDatabase.LoadAssetAtPath<KeywordManager>(KeywordManagerPath);
        }

        private void UpdateKeywords(KeywordManager manager)
        {
            for (int i = 0; i < _keywords.Length; i++)
            {
                Keyword keyword = manager.keywordList.Find(x => x.keywordName == _keywords[i].keywordName);
                _keywords[i].keywordValue = keyword.keywordValue;
            }
        }

        public WeightContainer WeightData
        {
            get { return _weightData; }
            set { _weightData = value; }
        }
    }
}
