using System.Collections.Generic;
using Editor.CardData;
using UnityEngine;

namespace Editor.Tests.Mocks
{
    public class MockCardSetData
    {
        public Color SetLabelColor { get; set; }
        public string CardSetName { get; set; } = "Default Mock Set";
        public int NumberOfCards { get; set; } = 10;

        public float CommonPercentage { get; set; } = 0.5f;
        public float UncommonPercentage { get; set; } = 0.3f;
        public float RarePercentage { get; set; } = 0.15f;
        public float HyperRarePercentage { get; set; } = 0.05f;

        public List<MockCardData> CardsInSet { get; set; } = new List<MockCardData>();
        public List<MockCardData> CommonCardsInSet { get; set; } = new List<MockCardData>();
        public List<MockCardData> UncommonCardsInSet { get; set; } = new List<MockCardData>();
        public List<MockCardData> RareCardsInSet { get; set; } = new List<MockCardData>();
        public List<MockCardData> HyperRareCardsInSet { get; set; } = new List<MockCardData>();

        public int CommonLimit => CalculateLimit(CommonPercentage);
        public int UncommonLimit => CalculateLimit(UncommonPercentage);
        public int RareLimit => CalculateLimit(RarePercentage);
        public int HyperRareLimit => CalculateLimit(HyperRarePercentage);

        public void ChangeSetName(string newSetName)
        {
            if (string.IsNullOrEmpty(newSetName))
            {
                return;
            }
            CardSetName = newSetName;
        }

        private int CalculateLimit(float percentage)
        {
            if ((int)(NumberOfCards * percentage) < 1)
            {
                return 0;
            }

            return (int)(NumberOfCards * percentage);
        }

        public void AddCardToSet(MockCardData cardData)
        {
            if (CardsInSet.Count >= NumberOfCards)
            {
                Debug.LogError($"The set already has the maximum number of {NumberOfCards} cards.");
                return;
            }

            switch (cardData.Rarity)
            {
                case CardRarity.Common:
                    if (CommonCardsInSet.Count < CommonLimit)
                        CommonCardsInSet.Add(cardData);
                    else
                        Debug.LogError("Common card limit reached.");
                    break;

                case CardRarity.Uncommon:
                    if (UncommonCardsInSet.Count < UncommonLimit)
                        UncommonCardsInSet.Add(cardData);
                    else
                        Debug.LogError("Uncommon card limit reached.");
                    break;

                case CardRarity.Rare:
                    if (RareCardsInSet.Count < RareLimit)
                        RareCardsInSet.Add(cardData);
                    else
                        Debug.LogError("Rare card limit reached.");
                    break;

                case CardRarity.HyperRare:
                    if (HyperRareCardsInSet.Count < HyperRareLimit)
                        HyperRareCardsInSet.Add(cardData);
                    else
                        Debug.LogError("HyperRare card limit reached.");
                    break;

                default:
                    Debug.LogError("Invalid Rarity Type.");
                    return;
            }

            CardsInSet.Add(cardData);
            cardData.CardSetName = CardSetName;
            cardData.CardNumber = CardsInSet.Count; // Sequential assignment
        }

        public void RemoveCardFromSet(MockCardData cardData)
        {
            CardsInSet.Remove(cardData);
            CommonCardsInSet.Remove(cardData);
            UncommonCardsInSet.Remove(cardData);
            RareCardsInSet.Remove(cardData);
            HyperRareCardsInSet.Remove(cardData);

            cardData.CardSetName = "None";
            ReassignCardNumbers();
        }

        public void AddMultipleCardsToSet(List<MockCardData> cardsToAdd)
        {
            foreach (MockCardData card in cardsToAdd)
            {
                AddCardToSet(card);
            }
        }

        public void RemoveMultipleCardsFromSet(List<MockCardData> cardsToRemove)
        {
            foreach (MockCardData card in cardsToRemove)
            {
                RemoveCardFromSet(card);
            }
        }

        private void ReassignCardNumbers()
        {
            for (int i = 0; i < CardsInSet.Count; i++)
            {
                CardsInSet[i].CardNumber = i + 1;
            }
        }
    }
}