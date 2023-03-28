using Een.Model;

namespace Een.Test.Model
{
    [TestFixture]
    public class DeckTests
    {
        [Test]
        public void New_ShouldReturnQueueWithAllCards()
        {
            // Arrange
            const int expectedCount = 108;

            // Act
            Queue<Card> deck = Deck.New();

            // Assert
            Assert.That(deck, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public void Shuffle_ShouldReturnQueueWithSameCards()
        {
            // Arrange
            Queue<Card> deck = Deck.New();

            // Act
            Queue<Card> shuffledDeck = Deck.Shuffle(deck);
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shuffledDeck, Has.Count.EqualTo(deck.Count));
                Assert.That(deck.All(card => shuffledDeck.Contains(card)), Is.True);
            });
        }
    }
}