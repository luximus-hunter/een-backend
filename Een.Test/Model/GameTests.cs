using Een.Model;

namespace Een.Test.Model
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            const string password = "testpassword";
            const int maxPlayers = 4;

            // Act
            Game game = new(password, maxPlayers);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(game.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(game.CheckPassword(password), Is.True);
                Assert.That(game.MaxPlayers, Is.EqualTo(maxPlayers));
                Assert.That(game.Players, Is.Not.Null);
                Assert.That(game.DrawPile, Is.Not.Null);
                Assert.That(game.DiscardPile, Is.Not.Null);
                Assert.That(game.OnTurn, Is.EqualTo(Guid.Empty));
                Assert.That(game.Running, Is.False);
                Assert.That(game.DrawBuffer, Is.EqualTo(0));
            });
        }

        [Test]
        public void HasPlayer_ShouldReturnTrueIfPlayerExists()
        {
            // Arrange
            Game game = new("testpassword", 4);
            Player player = new("testplayer");

            // Act
            game.Players.Enqueue(player);

            // Assert
            Assert.That(game.HasPlayer(player.Id), Is.True);
        }

        [Test]
        public void GetPlayer_ShouldReturnPlayerIfPlayerExists()
        {
            // Arrange
            Game game = new("testpassword", 4);
            Player player = new("testplayer");

            // Act
            game.Players.Enqueue(player);

            // Assert
            Assert.That(game.GetPlayer(player.Id), Is.EqualTo(player));
        }

        [Test]
        public void RemovePlayer_ShouldRemovePlayerIfPlayerExists()
        {
            // Arrange
            Game game = new("testpassword", 4);
            Player player = new("testplayer");
            game.Players.Enqueue(player);

            // Act
            game.RemovePlayer(player.Id);

            // Assert
            Assert.That(game.HasPlayer(player.Id), Is.False);
        }
    }
}