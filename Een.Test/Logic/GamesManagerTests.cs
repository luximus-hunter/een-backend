using Een.Logic;
using Een.Model;

namespace Een.Test.Logic
{
    [TestFixture]
    public class GamesManagerTests
    {
        [Test]
        public void New_WhenCalled_ReturnsGame()
        {
            // Arrange
            GamesManager.Empty();
            const string password = "password";
            const int maxPlayers = 4;

            // Act
            Game game = GamesManager.New(password, maxPlayers);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(game, Is.Not.Null);
                Assert.That(game.CheckPassword(password));
                Assert.That(game.MaxPlayers, Is.EqualTo(maxPlayers));
            });
        }

        [Test]
        public void Exists_WhenCalledWithExistingGame_ReturnsTrue()
        {
            // Arrange
            GamesManager.Empty();
            Game game = GamesManager.New("password", 4);

            // Act
            bool result = GamesManager.Exists(game.Id);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Exists_WhenCalledWithNonExistingGame_ReturnsFalse()
        {
            // Arrange
            GamesManager.Empty();
            Guid id = Guid.NewGuid();

            // Act
            bool result = GamesManager.Exists(id);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Find_WhenCalledWithExistingGame_ReturnsGame()
        {
            // Arrange
            GamesManager.Empty();
            Game game = GamesManager.New("password", 4);

            // Act
            Game? result = GamesManager.Find(game.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(game));
            });
        }

        [Test]
        public void Find_WhenCalledWithNonExistingGame_ReturnsNull()
        {
            // Arrange
            GamesManager.Empty();
            Guid id = Guid.NewGuid();

            // Act
            Game? result = GamesManager.Find(id);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void FindByPlayerId_WhenCalledWithExistingPlayer_ReturnsGame()
        {
            // Arrange
            GamesManager.Empty();
            Game game = GamesManager.New("password", 4);
            Player player = new("player");
            game.Players.Enqueue(player);

            // Act
            Game? result = GamesManager.FindByPlayerId(player.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(game));
            });
        }

        [Test]
        public void FindByPlayerId_WhenCalledWithNonExistingPlayer_ReturnsNull()
        {
            // Arrange
            GamesManager.Empty();
            Guid id = Guid.NewGuid();

            // Act
            Game? result = GamesManager.FindByPlayerId(id);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Purge_WhenCalledWithEmptyGames_RemovesAllGames()
        {
            // Arrange
            GamesManager.Empty();
            GamesManager.New("password", 4);
            GamesManager.New("password", 4);
            GamesManager.New("password", 4);

            // Act
            GamesManager.Purge();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(GamesManager.Games, Is.Empty);
                Assert.That(GamesManager.Processors, Is.Empty);
            });
        }

        [Test]
        public void Purge_WhenCalledWithNonEmptyGames_RemovesOnlyEmptyGames()
        {
            // Arrange
            GamesManager.Empty();
            Game game1 = GamesManager.New("password", 4);
            Game game2 = GamesManager.New("password", 4);
            Player player = new("player");
            game2.Players.Enqueue(player);

            // Act
            GamesManager.Purge();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(GamesManager.Games, Has.Count.EqualTo(1));
                Assert.That(GamesManager.Processors, Has.Count.EqualTo(1));
                Assert.That(GamesManager.Games.ContainsKey(game2.Id), Is.True);
                Assert.That(GamesManager.Processors.ContainsKey(game2.Id), Is.True);
                Assert.That(GamesManager.Games.ContainsKey(game1.Id), Is.False);
                Assert.That(GamesManager.Processors.ContainsKey(game1.Id), Is.False);
            });
        }
        
        [Test]
        public void Processor_WhenCalledWithExistingGame_ReturnsProcessor()
        {
            // Arrange
            GamesManager.Empty();
            Game game = GamesManager.New("password", 4);

            // Act
            Processor result = GamesManager.Processor(game.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Processor_WhenCalledWithNonExistingGame_ThrowsException()
        {
            // Arrange
            GamesManager.Empty();
            Guid id = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => GamesManager.Processor(id));
        }
    }
}