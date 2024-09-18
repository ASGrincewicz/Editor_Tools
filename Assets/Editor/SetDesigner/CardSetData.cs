using System.Collections.Generic;
using Editor.CardData;
using Editor.Utilities;
using UnityEngine;

namespace Editor.SetDesigner
{
    [CreateAssetMenu(fileName = "Card Set Data", menuName = "Card Set", order = 0)]
    public class CardSetData : ScriptableObject
    {
        public Color isSetLabelColor;
        [SerializeField] private CardSetType _cardSetType;
        [SerializeField] private string _cardSetName;
        [SerializeField] private int _numberOfCards;
        [SerializeField] private float _commonPercentage;
        [SerializeField] private float _uncommonPercentage;
        [SerializeField] private float _rarePercentage;
        [SerializeField] private float _hyperRarePercentage;
        [SerializeField] private List<CardDataSO> _cardsInSet;
        private List<CardDataSO> _commonCardsInSet;
        private List<CardDataSO> _uncommonCardsInSet;
        private List<CardDataSO> _rareCardsInSet;
        private List<CardDataSO> _hyperRareCardsInSet;
        private List<CardDataSO> _promoCardsInSet;
        private List<CardDataSO> _kickStarterCardsInSet;

       public CardSetType CardSetType
        {
            get { return _cardSetType; }
            set { _cardSetType = value; }
        }

        public string CardSetName
        {
            get{return _cardSetName;}
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _cardSetName = value;
                }
            }
        }

        public int NumberOfCards
        {
            get{return _numberOfCards;}
            set
            {
                if (value > 0)
                {
                    _numberOfCards = value;
                }
            }
        }

        public int CommonLimit
        {
            get
            {
                return (int)(_numberOfCards * _commonPercentage);
            }
        }

        public int UncommonLimit
        {
            get
            {
                return (int)(_numberOfCards * _uncommonPercentage);
            }
        }

        public int RareLimit
        {
            get
            {
                return (int)(_numberOfCards * _rarePercentage);
            }
        }

        public int HyperRareLimit
        {
            get
            {
                return (int)(_numberOfCards * _hyperRarePercentage);
            }
        }

        public List<CardDataSO> CardsInSet
        {
            get { return _cardsInSet ??= new List<CardDataSO>(); }
            set { _cardsInSet =  value; }
        }
        
        public void AddMultipleCardsToSet(List<CardDataSO> cardsToAdd)
        {
            foreach (CardDataSO card in cardsToAdd)
            {
                AddCardToSet(card);
            }
        }
        
        public void AddCardToSet(CardDataSO cardData)
        {
            ErrorHandler.TryToGetCard(cardData);
            if (!_cardsInSet.Contains(cardData) && _cardsInSet.Count < _numberOfCards)
            {
                _cardsInSet.Add(cardData);
                AssignSetToCard(cardData);
                AssignNumberToCard(cardData);
            }
        }
        
        public void RemoveMultipleCardsFromSet(List<CardDataSO> cardsToRemove)
        {
            foreach (CardDataSO card in cardsToRemove)
            {
                RemoveCardFromSet(card);
            }
        }
        public void RemoveCardFromSet(CardDataSO cardData)
        {
            ErrorHandler.TryToGetCardFromList(CardsInSet, cardData);
            UnassignNumberFromCard(cardData);
            UnassignSetFromCard(cardData);
            _cardsInSet.Remove(cardData);
        }

        public void AssignNumberToCard(CardDataSO cardData)
        {
            ErrorHandler.TryToGetCard(cardData);
            cardData.CardNumber = CardsInSet.IndexOf(cardData) + 1;
            
        }

        public void UnassignNumberFromCard(CardDataSO cardData)
        {
            cardData.CardNumber = 0;
        }

        public void AssignSetToCard(CardDataSO cardData)
        {
            ErrorHandler.TryToGetCard(cardData);
            cardData.CardSetName = _cardSetName;
        }

        public void UnassignSetFromCard(CardDataSO cardData)
        {
            cardData.CardSetName = "None";
        }
        
    }
}