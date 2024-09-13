using System.Collections.Generic;
using Editor.CardData;
using Editor.Utilities;
using UnityEngine;

namespace Editor.SetDesigner
{
    [CreateAssetMenu(fileName = "Card Set Data", menuName = "Card Set", order = 0)]
    public class CardSetData : ScriptableObject
    {
        [SerializeField] private CardSetType cardSetType;
        [SerializeField] private string _cardSetName;
        [SerializeField] private int _numberOfCards;
        [SerializeField] private List<CardSO> _cardsInSet;

        public CardSetType CardSetType
        {
            get { return cardSetType; }
            set { cardSetType = value; }
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

        public List<CardSO> CardsInSet
        {
            get { return _cardsInSet ??= new List<CardSO>(); }
            set { _cardsInSet =  value; }
        }
        
        public void AddMultipleCardsToSet(List<CardSO> cardsToAdd)
        {
            foreach (CardSO card in cardsToAdd)
            {
                AddCardToSet(card);
            }
        }
        
        public void AddCardToSet(CardSO card)
        {
            ErrorHandler.TryToGetCard(card);
            if (!_cardsInSet.Contains(card) && _cardsInSet.Count < _numberOfCards)
            {
                _cardsInSet.Add(card);
            }
        }
        
        public void RemoveMultipleCardsFromSet(List<CardSO> cardsToRemove)
        {
            foreach (CardSO card in cardsToRemove)
            {
                RemoveCardFromSet(card);
            }
        }
        public void RemoveCardFromSet(CardSO card)
        {
            ErrorHandler.CheckListForCard(CardsInSet, card);
            _cardsInSet.Remove(card);
        }
    }
}