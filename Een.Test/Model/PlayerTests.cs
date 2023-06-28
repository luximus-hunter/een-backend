using Een.Model;

namespace Een.Test.Model
{
    public class PlayerTests
    {
        [Test]
        public void AddCard_ShouldAddCardToCardsList()
        {
            // Arrange
            Player player = new("test");
            Card card = new(CardColor.Blue, CardValue.C1);

            // Act
            player.AddCard(card);

            // Assert
            Assert.That(player.Cards, Does.Contain(card));
        }

        [Test]
        public void HasCard_ShouldReturnTrueIfPlayerHasCard()
        {
            // Arrange
            Player player = new("test");
            Card card = new(CardColor.Blue, CardValue.C1);
            player.AddCard(card);

            // Act
            bool result = player.HasCard(card);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasCard_ShouldReturnTrueIfPlayerHasWildCard()
        {
            // Arrange
            Player player = new("test");
            Card card = new(CardColor.Green, CardValue.Wild);
            player.AddCard(card);

            // Act
            bool result = player.HasCard(card);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasCard_ShouldReturnFalseIfPlayerDoesNotHaveCard()
        {
            // Arrange
            Player player = new("test");
            Card card = new(CardColor.Blue, CardValue.C1);

            // Act
            bool result = player.HasCard(card);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void RemoveCard_ShouldRemoveCardFromCardsList()
        {
            // Arrange
            Player player = new("test");
            Card card = new(CardColor.Blue, CardValue.C1);
            player.AddCard(card);

            // Act
            player.RemoveCard(card);

            // Assert
            Assert.That(player.Cards, Does.Not.Contain(card));
        }

        [Test]
        public void RemoveCard_ShouldRemoveWildCardFromCardsList()
        {
            // Arrange
            Player player = new("test");
            Card card = new(CardColor.Red, CardValue.Wild);
            player.AddCard(card);

            // Act
            player.RemoveCard(card);

            // Assert
            Assert.That(player.Cards, Does.Not.Contain(card));
        }
    }
}