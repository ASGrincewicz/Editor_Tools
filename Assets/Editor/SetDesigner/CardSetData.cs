using System;
using System.Collections.Generic;
using System.Net;
using Editor.CardData;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.SetDesigner
{
    [CreateAssetMenu(fileName = "Card Set Data", menuName = "Card Set", order = 0)]
    public class CardSetData : ScriptableObject
    {
        public Color setLabelColor;
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
            get
            {
                if (CardSetType == CardSetType.None)
                {
                    _numberOfCards = AssetDatabase.FindAssets("t:CardDataSO").Length;
                }
                return _numberOfCards;
            }
            set
            {
                if (value > 0)
                {
                    _numberOfCards = value;
                }
            }
        }

        public float CommonPercentage
        {
            get { return _commonPercentage; }
            set { _commonPercentage = value; }
        }

        public float UncommonPercentage
        {
            get { return _uncommonPercentage; }
            set { _uncommonPercentage = value; }
        }

        public float RarePercentage
        {
            get { return _rarePercentage; }
            set { _rarePercentage = value; }
        }

        public float HyperRarePercentage
        {
            get { return _hyperRarePercentage; }
            set { _hyperRarePercentage = value; }
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
            if (cardData.CardSetName != "None")
            {
                Debug.LogError($"{cardData.CardName} is already in {cardData.CardSetName}.");
                return;
            }
            if (_cardsInSet.Count >= _numberOfCards)
            {
                Debug.LogError($"{_cardSetName} already has {_numberOfCards} Cards.");
                return;
            }
            if (_cardsInSet.Contains(cardData))
            {
                Debug.LogError($"{cardData.CardName} is already assigned to CardSet {cardData.CardSetName}");
                return;
            }
            if(CheckSetForCardRarityCapacity(cardData))
            {
               
                _cardsInSet.Add(cardData);
                AssignSetToCard(cardData);
                AssignNumberToCard(cardData);
            }
            else
            {
                Debug.LogError($"{_cardSetName} has reached the limit for {cardData.Rarity} cards.");
            }
            
        }

        private bool CheckSetForCardRarityCapacity(CardDataSO cardData)
        {
            switch (cardData.Rarity)
            {
                case CardRarity.None:
                    break;
                case CardRarity.Common:
                    if (_commonCardsInSet.Count < CommonLimit)
                    {
                        return true;
                    }
                    break;
                case CardRarity.Uncommon:
                    if (_uncommonCardsInSet.Count < UncommonLimit)
                    {
                        return true;
                    }
                    break;
                case CardRarity.Rare:
                    if (_rareCardsInSet.Count < RareLimit)
                    {
                        return true;
                    }
                    break;
                case CardRarity.HyperRare:
                    if (_hyperRareCardsInSet.Count < HyperRareLimit)
                    {
                        return true;
                    }
                    break;
               default:
                    return false;
            }
            return false;
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
            _cardsInSet.Remove(cardData);
            UnassignNumberFromCard(cardData);
            UnassignSetFromCard(cardData);
            ReAssignNumbersToCards();
        }

        private void AssignNumberToCard(CardDataSO cardData)
        {
            ErrorHandler.TryToGetCard(cardData);
            cardData.CardNumber = CardsInSet.IndexOf(cardData) + 1;
        }

        private void ReAssignNumbersToCards()
        {
            foreach (CardDataSO card in CardsInSet)
            {
               AssignNumberToCard(card);
            }
        }

        private void UnassignNumberFromCard(CardDataSO cardData)
        {
            cardData.CardNumber = 0;
        }

        private void AssignSetToCard(CardDataSO cardData)
        {
            ErrorHandler.TryToGetCard(cardData);
            cardData.CardSetName = _cardSetName;
        }

        private void UnassignSetFromCard(CardDataSO cardData)
        {
            cardData.CardSetName = "None";
        }
        
    }
}