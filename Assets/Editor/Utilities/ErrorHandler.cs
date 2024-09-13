using System;
using System.Collections.Generic;
using Editor.AttributesWeights;
using Editor.CardData;
using UnityEngine;

namespace Editor.Utilities
{
    public static class ErrorHandler
    {
        public static CardSO TryToGetCard(CardSO card)
        {
            try
            {
                if (ReferenceEquals(card, null))
                {
                    throw new CardSOIsNullException("CardSO is null.");
                }
                return card;
            }
            catch (CardSOIsNullException exception)
            {
                Debug.LogException(exception, card);
                return ScriptableObject.CreateInstance<CardSO>();
            }
        }

        public static void TryToGetWeightData(WeightContainer weightData)
        {
            try
            {
                if (ReferenceEquals(weightData, null))
                {
                    throw new WeightDataIsNullException($"{nameof(weightData)} is null.");
                }
            }
            catch (WeightDataIsNullException exception)
            {
                Debug.LogException(exception, weightData);
            }
        }

        public static void TryToGetCardFromList(List<CardSO> listToCheck, CardSO cardToCheck)
        {
            try
            {
                TryToGetCard(cardToCheck);
                if (!listToCheck.Contains(cardToCheck))
                {
                    throw new CardNotInCollectionException($"{cardToCheck} is not in {listToCheck}.");
                }
            }
            catch (CardSOIsNullException exception)
            {
                Debug.LogException(exception, cardToCheck);
            }
        }

        public static bool VerifyCardNotInList(List<CardSO> listToCheck, CardSO cardToCheck)
        {
            try
            {
                if (listToCheck.Contains(cardToCheck))
                {
                    throw new CardAlreadyInListException($"{cardToCheck} is in {listToCheck}.");
                }

                return true;
            }
            catch (CardAlreadyInListException exception)
            {
                Debug.LogException(exception, cardToCheck);
                return false;
            }
        }
    }
}