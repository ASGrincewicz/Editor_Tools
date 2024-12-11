using Editor.CardData;
using NUnit.Framework;

namespace Editor.Tests
{
    [TestFixture]
    public class CardInspectorLogicTest
    {
        private CardInspectorLogic _cardInspectorLogic;
        
        [SetUp]
        public void Setup()
        {
            _cardInspectorLogic = new CardInspectorLogic();
        }
        
        [Test]
        public void FormatPropertyLabel_ShouldReturnFormattedLabel_WhenValidDataIsPassed()
        {
            // Arrange
            const string expectedLabel = "Card Name";
            const string propertyName = "CardName";
            
            // Act
            (string,string) result = _cardInspectorLogic.FormatPropertyLabel(propertyName, expectedLabel);
            
            // Assert
            Assert.AreEqual((propertyName, expectedLabel), result);
        }
        
        [Test]
        public void ShouldDrawCardStatData_ShouldReturnTrue_WhenHasStatsIsTrue()
        {
            // Arrange
            bool hasStats = true;
            
            // Act
            bool result = _cardInspectorLogic.ShouldDrawCardStatData(hasStats);
            
            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void ShouldDrawCardStatData_ShouldReturnFalse_WhenHasStatsIsFalse()
        {
            // Arrange
            bool hasStats = false;
            
            // Act
            bool result = _cardInspectorLogic.ShouldDrawCardStatData(hasStats);
            
            // Assert
            Assert.IsFalse(result);
        }
    }
}