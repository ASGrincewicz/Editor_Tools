using Editor.AttributesWeights;
using Editor.KeywordSystem;
using UnityEditor;
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
            get { return _keywords; }
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

        public int GetKeywordsTotalValue()
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

        public void GetCurrentKeywordInfo()
        {
            KeywordManager manager =
                AssetDatabase.LoadAssetAtPath<KeywordManager>(
                    "Assets/Data/Scriptable Objects/Keywords/KeywordManager.asset");
            if(!ReferenceEquals(manager, null))
            {
                for(int i=0; i<Keywords.Length; i++)
                {
                     Keyword temp = manager.keywordList.Find(x => x.keywordName == _keywords[i].keywordName);
                     _keywords[i].keywordValue = temp.keywordValue;
                     Debug.Log($"{_keywords[i].keywordName} value was updated to {temp.keywordValue}");
                }
            }
        }

        public WeightContainer WeightData
        {
            get { return _weightData; }
            set { _weightData = value; }
        }
    }
}
