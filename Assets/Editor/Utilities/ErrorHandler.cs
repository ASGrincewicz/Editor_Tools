using Editor.AttributesWeights;
using Editor.CardData;
using UnityEngine;

namespace Editor.Utilities
{
    public static class ErrorHandler
    {
        public static void TryToGetCard(CardSO card)
        {
            try
            {
                if (ReferenceEquals(card, null))
                {
                    throw new CardSOIsNullException("CardSO is null.");
                }
            }
            catch (CardSOIsNullException exception)
            {
                Debug.LogException(exception, card);
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
    }
}