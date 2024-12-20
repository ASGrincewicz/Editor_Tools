using System.Collections.Generic;
using Editor.CardData.CardTypes;
using Editor.CardData.Stats;
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
    public class CardDataSO : ScriptableObject
    {
        private const string KeywordManagerPath = "Assets/Data/Scriptable Objects/Keywords/KeywordManager.asset";
        //[SerializeField] private WeightContainer _weightData;
        [SerializeField] private string _cardSetName = "None";
        [SerializeField] private int _cardNumber = 0;
        [SerializeField] private CardRarity _rarity;
        [SerializeField] private int _cost;
        [SerializeField] private CardTypeDataSO _cardTypeData;
        [SerializeField] private string _cardName;
        [SerializeField] private Texture2D _artWork;
        [SerializeField] private Keyword[] _keywords;
        [SerializeField][Multiline]
        private string _cardText;
        // Stats
        [SerializeField] private List<CardStat> _stats;

        public string CardSetName
        {
            get
            {
                return string.IsNullOrEmpty(_cardSetName) ? "None" : _cardSetName;
            }
            set { _cardSetName = value; }
        }
        public int CardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }
        
        public CardRarity Rarity
        {
            get { return _rarity; }
            set { _rarity = value; }
        }
        public int CardCost
        {
            get { return _cost; }
            set { _cost = value; }
        }
       
        /// Type of the card.
       
        public CardTypeDataSO CardTypeDataSO 
        {
            get
            {
                return _cardTypeData;
            }
            set
            {
                Debug.Log($"CardTypeDataSO: {_cardTypeData}");
                _cardTypeData = value;
                
            }
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

        public List<CardStat> Stats
        {
            get { return _stats; }
            set { _stats = value; }
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
    }
}
