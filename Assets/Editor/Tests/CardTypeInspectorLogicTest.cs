using Editor.CardData.CardTypes;
using NUnit.Framework;

namespace Editor.Tests
{
    public class CardTypeInspectorLogicTests
    {
        private CardTypeInspectorLogic _cardTypeInspectorLogic;

        [SetUp]
        public void Setup()
        {
            // Initialize CardTypeInspectorLogic before each test
            _cardTypeInspectorLogic = new CardTypeInspectorLogic();
        }

        [Test]
        public void FormatPropertyLabel_ShouldReturnFormattedLabel_WhenValidDataIsPassed()
        {
            // Arrange
            string propertyName = "Card Type Name";
            string propertyValue = "Magic Card";

            // Act
            (string, string) result = _cardTypeInspectorLogic.FormatPropertyLabel(propertyName, propertyValue);

            // Assert
            Assert.AreEqual((propertyName, propertyValue), result);
        }

        [Test]
        public void ShouldDrawCardStatData_ShouldReturnTrue_WhenHasStatsIsTrue()
        {
            // Arrange
            bool hasStats = true;

            // Act
            bool result = _cardTypeInspectorLogic.ShouldDrawCardStatData(hasStats);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldDrawCardStatData_ShouldReturnFalse_WhenHasStatsIsFalse()
        {
            // Arrange
            bool hasStats = false;

            // Act
            bool result = _cardTypeInspectorLogic.ShouldDrawCardStatData(hasStats);

            // Assert
            Assert.IsFalse(result);
        }
    }
}