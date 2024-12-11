using System.Collections.Generic;
using Editor.CardData;
using Editor.CardData.Stats;
using Editor.Tests.Mocks;
using NUnit.Framework;
using UnityEngine;
using Keyword = Editor.Tests.Mocks.Keyword;

namespace Editor.Tests
{
    public class CardDataSOTests
    {
        private MockCardData cardData;

        [SetUp]
        public void Setup()
        {
            // Create a new instance of CardDataSO
            cardData = new MockCardData();
        }

        [Test]
        public void TestCardSetNameDefault()
        {
            Assert.AreEqual("None", cardData.CardSetName);
        }

        [Test]
        public void TestCardSetNameAssignment()
        {
            cardData.CardSetName = "TestSet";
            Assert.AreEqual("TestSet", cardData.CardSetName);
        }

        [Test]
        public void TestCardNumberAssignment()
        {
            cardData.CardNumber = 5;
            Assert.AreEqual(5, cardData.CardNumber);
        }

        [Test]
        public void TestCardRarityAssignment()
        {
            CardRarity expectedRarity = CardRarity.Rare;
            cardData.Rarity = expectedRarity;
            Assert.AreEqual(expectedRarity, cardData.Rarity);
        }

        [Test]
        public void TestCardCostAssignment()
        {
            cardData.CardCost = 10;
            Assert.AreEqual(10, cardData.CardCost);
        }

        [Test]
        public void TestCardTypeDataAssignment()
        {
            MockCardTypeData cardTypeData = new();
            cardData.MockCardTypeData = cardTypeData;
            Assert.AreEqual(cardTypeData, cardData.MockCardTypeData);
        }

        [Test]
        public void TestCardNameAssignment()
        {
            cardData.CardName = "Test Card";
            Assert.AreEqual("Test Card", cardData.CardName);
        }

        [Test]
        public void TestArtWorkAssignment()
        {
            Texture2D texture = new(2, 2);
            cardData.ArtWork = texture;
            Assert.AreEqual(texture, cardData.ArtWork);
        }

        [Test]
        public void TestKeywordsDefault()
        {
            Keyword[] keywords = cardData.Keywords;
            Assert.IsNotNull(keywords);
            Assert.AreEqual(3, keywords.Length);
        }

        [Test]
        public void TestCardTextAssignment()
        {
            cardData.CardText = "Test Text";
            Assert.AreEqual("Test Text", cardData.CardText);
        }

        [Test]
        public void TestStatsAssignment()
        {
            List<CardStat> stats = new() { new CardStat(), new CardStat() };
            cardData.Stats = stats;
            Assert.AreEqual(stats, cardData.Stats);
        }

        [Test]
        public void TestGetKeywordsSumStringWithNoKeywords()
        {
            cardData.Keywords = new Keyword[0];
            string result = cardData.GetKeywordsSumString();
            Assert.AreEqual("No Keywords Assigned to this card.", result);
        }

        [Test]
        public void TestAssignAndRetrieveKeywordsUsingMockKeywordManager()
        {
            // Arrange: Create and assign mock keywords using MockKeywordManager
            MockKeywordManager keywordManager = new();
            Keyword keyword1 = new() { KeywordName = "MockedKeyword1", Definition = "Test keyword 1" };
            Keyword keyword2 = new() { KeywordName = "MockedKeyword2", Definition = "Test keyword 2" };
            
            keywordManager.AddKeyword(keyword1);
            keywordManager.AddKeyword(keyword2);

            // Act: Assign keywords to the mock card
            cardData.Keywords = keywordManager.GetKeywords();

            // Assert: Verify the keywords are assigned correctly
            Assert.AreEqual(2, cardData.Keywords.Length);
            Assert.AreEqual(keyword1, cardData.Keywords[0]);
            Assert.AreEqual(keyword2, cardData.Keywords[1]);
        }

        [Test]
        public void TestGetKeywordsSumStringAssignedKeywords()
        {
            // Arrange: MockKeywordManager with some predefined keywords
            MockKeywordManager keywordManager = new();
            keywordManager.AddKeyword(new Keyword { KeywordName = "Keyword1", KeywordValue = 1});
            keywordManager.AddKeyword(new Keyword { KeywordName = "Keyword2", KeywordValue = 2});

            cardData.Keywords = keywordManager.GetKeywords();

            // Act: Generate the keywords summary string
            string result = cardData.GetKeywordsSumString();

            // Assert: Verify that the correct summary is generated
            Assert.AreEqual("Keyword(1), Keyword(2)", result); // Adjust based on the method implementation
        }

        [Test]
        public void TestRemoveKeywordUsingMockKeywordManager()
        {
            // Arrange: Create a MockKeywordManager and add/remove keywords
            MockKeywordManager keywordManager = new();
            Keyword keyword1 = new() { KeywordName = "RemovableKeyword", Definition = "This will be removed" };

            keywordManager.AddKeyword(keyword1);
            keywordManager.AddKeyword(new Keyword { KeywordName = "PersistentKeyword", Definition = "This stays" });
            keywordManager.RemoveKeyword(keyword1);

            // Act: Assign remaining keywords to the card
            cardData.Keywords = keywordManager.GetKeywords();

            // Assert: Verify that the removed keyword is no longer present
            Assert.AreEqual(1, cardData.Keywords.Length);
            Assert.AreEqual("PersistentKeyword", cardData.Keywords[0].KeywordName);
        }

        [Test]
        public void TestKeywordsSumWithEmptyKeywords()
        {
            cardData.Keywords = new Keyword[0];
            string result = cardData.GetKeywordsSumString();
            Assert.AreEqual("No Keywords Assigned to this card.", result);
        }

        [Test]
        public void TestAddDuplicateKeywordsUsingMockKeywordManager()
        {
            // Arrange: Initialize MockKeywordManager with duplicates
            MockKeywordManager keywordManager = new();
            Keyword duplicateKeyword = new() { KeywordName = "DuplicateKeyword" };

            keywordManager.AddKeyword(duplicateKeyword);
            keywordManager.AddKeyword(duplicateKeyword); // Duplicate addition

            cardData.Keywords = keywordManager.GetKeywords();

            // Assert: Assuming duplicates are not allowed (adjust based on implementation)
            Assert.AreEqual(1, cardData.Keywords.Length, "Duplicates should not be added.");
            Assert.AreEqual("DuplicateKeyword", cardData.Keywords[0].KeywordName);
        }
    }
}