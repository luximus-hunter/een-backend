using Een.Model;

namespace Een.Test.Model
{
    [TestFixture]
    public class MoveTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            Card card = new(CardColor.Blue, CardValue.C1);
            const MoveAction action = MoveAction.Play;

            // Act
            Move move = new(card, action);
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(move.Card, Is.EqualTo(card));
                Assert.That(move.Action, Is.EqualTo(action));
            });
        }
    }
}