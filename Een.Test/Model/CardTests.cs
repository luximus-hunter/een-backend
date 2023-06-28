using Een.Model;

namespace Een.Test.Model
{
    [TestFixture]
    public class CardTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            const CardColor color = CardColor.Blue;
            const CardValue value = CardValue.C1;

            // Act
            Card card = new(color, value);
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(card.Color, Is.EqualTo(color));
                Assert.That(card.Value, Is.EqualTo(value));
            });
        }
    }
}