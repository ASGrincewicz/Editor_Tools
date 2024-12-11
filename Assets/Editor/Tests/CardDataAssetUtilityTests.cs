using System.Collections.Generic;
using Editor.CardData;
using Editor.CardData.Stats;
using Editor.Tests.Mocks;
using NUnit.Framework;

namespace Editor.Tests
{
    [TestFixture]
    public class CardDataAssetUtilityTests
    {
        public MockCardDataAssetUtility MockCardDataAssetUtility { get; set; }
        
        [SetUp]
        public void SetUp()
        {
            MockCardDataAssetUtility = new();
            // Initialize or reset shared resources for each test
            MockCardDataAssetUtility.UnloadCard();
        }

        [Test]
        public void UpdateStats_ShouldUpdateCardStats()
        {
            // Arrange
            List<CardStat> newStats = new() { new CardStat("StatName", 1, "Description") };

            // Act
            MockCardDataAssetUtility.UpdateStats(newStats);

            // Assert
            Assert.AreEqual(newStats, MockCardDataAssetUtility.CardStats);
        }

        [Test]
        public void LoadCardTypeData_ValidCardToEdit_ShouldInitializeCardTypeData()
        {
            // Arrange
            MockCardTypeData mockCardTypeData = new();
            MockCardDataAssetUtility.CardToEdit = new();

            // Act
            MockCardTypeData result = MockCardDataAssetUtility.LoadCardTypeData(mockCardTypeData);

            // Assert
            Assert.AreEqual(mockCardTypeData, result);

            Assert.AreEqual(mockCardTypeData, result);
        }
        
        [Test]
        public void CreateNewCard_InitializesAndSavesCard_WhenCardToEditInitialized()
        {
            // Arrange
            List<CardStat> newStats = new() { new CardStat("Health", 10, "Description") };
            MockCardDataAssetUtility.CardToEdit = new(); 

            // Act
            MockCardDataAssetUtility.CreateNewCard(newStats);

            // In actual Unity tests, the AssetDatabase would need to be mocked or checked for file creation
            Assert.IsNotNull(MockCardDataAssetUtility.CardToEdit);
        }

        [Test]
        public void LoadKeywordManagerAsset_LoadsManager_Successfully()
        {
            // Act
            MockCardDataAssetUtility.LoadKeywordManagerAsset();

            // Assert
            Assert.NotNull(MockCardDataAssetUtility.MockKeywordManager);
        }

        [Test]
        public void RefreshKeywordsList_InitializesKeywordListsCorrectly()
        {
            // Arrange
            MockCardDataAssetUtility.MockKeywordManager = new();

            // Act
            MockCardDataAssetUtility.RefreshKeywordsList();

            // Since actual keywords are asset dependent, in real tests, you would check against expected results
            Assert.IsNotNull(MockCardDataAssetUtility.KeywordsList);

            Assert.IsNotNull(MockCardDataAssetUtility.KeywordNamesList);
        }

        [Test]
        public void UnloadCard_ResetsAllPropertiesToDefaults()
        {
            // Act
            if (MockCardDataAssetUtility.CardToEdit != null)
            {
                MockCardDataAssetUtility.UnloadCard();
            }

            // Assert each static property is reset as expected
            Assert.IsNull(MockCardDataAssetUtility.CardToEdit);

            Assert.IsNull(MockCardDataAssetUtility.CardTypeData);

            Assert.IsEmpty(MockCardDataAssetUtility.CardName);

            //Assert.AreEqual(CardRarity.None, MockCardDataAssetUtility.CardToEdit._rarity);

            Assert.IsNull(MockCardDataAssetUtility.SelectedKeywords);

            Assert.IsNull(MockCardDataAssetUtility.Artwork);
        }
    }
}