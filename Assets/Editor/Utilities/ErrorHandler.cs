using System.Collections.Generic;
using Editor.CardData;
using UnityEngine;

namespace Editor.Utilities
{
    public static class ErrorHandler
    {
        public static CardDataSO TryToGetCard(CardDataSO cardData)
        {
            try
            {
                if (ReferenceEquals(cardData, null))
                {
                    throw new CardSOIsNullException("CardDataSO is null.");
                }
                return cardData;
            }
            catch (CardSOIsNullException exception)
            {
                Debug.LogException(exception, cardData);
                return ScriptableObject.CreateInstance<CardDataSO>();
            }
        }
        
        public static void TryToGetCardFromList(List<CardDataSO> listToCheck, CardDataSO cardDataToCheck)
        {
            try
            {
                TryToGetCard(cardDataToCheck);
                if (!listToCheck.Contains(cardDataToCheck))
                {
                    throw new CardNotInCollectionException($"{cardDataToCheck} is not in {listToCheck}.");
                }
            }
            catch (CardSOIsNullException exception)
            {
                Debug.LogException(exception, cardDataToCheck);
            }
        }

        public static bool VerifyCardNotInList(List<CardDataSO> listToCheck, CardDataSO cardDataToCheck)
        {
            try
            {
                if (listToCheck.Contains(cardDataToCheck))
                {
                    throw new CardAlreadyInListException($"{cardDataToCheck} is in {listToCheck}.");
                }

                return true;
            }
            catch (CardAlreadyInListException exception)
            {
                Debug.LogException(exception, cardDataToCheck);
                return false;
            }
        }
    }
}