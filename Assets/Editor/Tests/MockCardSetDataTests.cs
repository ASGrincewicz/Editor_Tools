using System.Collections.Generic;
using Editor.CardData;
using Editor.Tests.Mocks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Editor.Tests
{
    [TestFixture]
    public class MockCardSetDataTests
    {
        private MockCardSetData _mockCardSetData;

        [SetUp]
        public void SetUp()
        {
            _mockCardSetData = new MockCardSetData
            {
                CardSetName = "Test Set",
                NumberOfCards = 10,
                CommonPercentage = 0.5f,
                UncommonPercentage = 0.3f,
                RarePercentage = 0.15f,
                HyperRarePercentage = 0.05f
            };
        }

        [Test]
        public void CardSetName_SetValidName_UpdatesName()
        {
            _mockCardSetData.CardSetName = "New Name";
            Assert.AreEqual("New Name", _mockCardSetData.CardSetName);
        }

        [Test]
        public void CardSetName_SetEmptyName_DoesNotChangeName()
        {
            string originalName = "Test Set";
            _mockCardSetData.ChangeSetName(string.Empty);

            Assert.AreEqual(originalName, _mockCardSetData.CardSetName);
        }

        [Test]
        public void CommonLimit_CalculatesCorrectLimit()
        {
            _mockCardSetData.NumberOfCards = 20;
            _mockCardSetData.CommonPercentage = 0.4f;

            Assert.AreEqual(8, _mockCardSetData.CommonLimit); // 20 * 0.4 = 8
        }

        [Test]
        public void AddCardToSet_AddsCardSuccessfully()
        {
            MockCardData cardMock = new()
            {
                CardName = "Test Card",
                Rarity = CardRarity.Common
            };

            _mockCardSetData.AddCardToSet(cardMock);

            Assert.Contains(cardMock, _mockCardSetData.CardsInSet);
        }

        [Test]
        public void AddCardToSet_ExceedsCardLimit_ThrowsError()
        {
            _mockCardSetData.NumberOfCards = 1;
            _mockCardSetData.CommonPercentage = 1;

            MockCardData cardMock = new() { CardName = "Card 1" , Rarity = CardRarity.Common};
            MockCardData cardMock2 = new() { CardName = "Card 2" , Rarity = CardRarity.Common};

            _mockCardSetData.AddCardToSet(cardMock);

            LogAssert.Expect(LogType.Error, $"The set already has the maximum number of {_mockCardSetData.NumberOfCards} cards.");
            _mockCardSetData.AddCardToSet(cardMock2);
        }

        [Test]
        public void RemoveCardFromSet_RemovesSuccessfully()
        {
            MockCardData cardMock = new() { CardName = "Test Card", Rarity = CardRarity.Common};

            _mockCardSetData.AddCardToSet(cardMock);
            _mockCardSetData.RemoveCardFromSet(cardMock);

            Assert.IsFalse(_mockCardSetData.CardsInSet.Contains(cardMock));
            Assert.AreEqual("None", cardMock.CardSetName);
        }

        [Test]
        public void AddMultipleCardsToSet_AddsAllSuccessfully()
        {
            List<MockCardData> cards = new()
            {
                new MockCardData { CardName = "Card 1", Rarity = CardRarity.Common },
                new MockCardData { CardName = "Card 2", Rarity = CardRarity.Common}
            };

            _mockCardSetData.AddMultipleCardsToSet(cards);

            Assert.AreEqual(2, _mockCardSetData.CardsInSet.Count);
        }

        [Test]
        public void RemoveMultipleCardsFromSet_RemovesAllSuccessfully()
        {
            List<MockCardData> cards = new()
            {
                new MockCardData { CardName = "Card 1", Rarity = CardRarity.Common},
                new MockCardData { CardName = "Card 2", Rarity = CardRarity.Common},
            };

            _mockCardSetData.AddMultipleCardsToSet(cards);
            _mockCardSetData.RemoveMultipleCardsFromSet(cards);

            Assert.AreEqual(0, _mockCardSetData.CardsInSet.Count);
        }
    }
}