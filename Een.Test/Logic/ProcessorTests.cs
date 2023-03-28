using Een.Logic;
using Een.Model;

namespace Een.Test.Logic
{
    [TestFixture]
    public class ProcessorTests
    {
        [Test]
        public void DrawCards_WhenPlayerDrawsCards_GivesCards()
        {
            // Arrange
            Game game = new("testpassword", 4);
            Player player = new("TestPlayer");
            game.DrawPile = Deck.New();
            Processor processor = new(game);

            // Act
            processor.DrawCards(player, 3);

            // Assert
            Assert.That(player.Cards, Has.Count.EqualTo(3));
        }

        [Test]
        public void DrawCards_WhenPlayerDrawsCardsWithEmptyPile_GivesCards()
        {
            // Arrange
            Game game = new("testpassword", 4);
            Player player = new("TestPlayer");
            game.DrawPile.Enqueue(new Card(CardColor.Blue, CardValue.C1));
            game.DrawPile.Enqueue(new Card(CardColor.Blue, CardValue.C2));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C3));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C4));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C5));
            Processor processor = new(game);

            // Act
            processor.DrawCards(player, 3);

            // Assert
            Assert.That(player.Cards, Has.Count.EqualTo(3));
        }

        [Test]
        public void TestRestockDrawPile()
        {
            // Arrange
            Game game = new("testpassword", 4);
            game.DrawPile.Enqueue(new Card(CardColor.Blue, CardValue.C1));
            game.DrawPile.Enqueue(new Card(CardColor.Blue, CardValue.C2));
            game.DrawPile.Enqueue(new Card(CardColor.Blue, CardValue.C3));
            game.DrawPile.Enqueue(new Card(CardColor.Blue, CardValue.C4));
            game.DrawPile.Enqueue(new Card(CardColor.Blue, CardValue.C5));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C6));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C7));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C8));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C9));
            game.DiscardPile.Push(new Card(CardColor.Blue, CardValue.C0));
            Processor processor = new(game);

            // Act
            processor.RestockDrawPile();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(game.DrawPile, Has.Count.EqualTo(9));
                Assert.That(game.DiscardPile.Peek().Value, Is.EqualTo(CardValue.C0));
            });
        }


        [Test]
        public void TestNextPlayer()
        {
            // Arrange
            Game game = new("testpassword", 4);
            game.Players.Enqueue(new Player("Player 1"));
            game.Players.Enqueue(new Player("Player 2"));
            game.Players.Enqueue(new Player("Player 3"));
            game.Players.Enqueue(new Player("Player 4"));
            Processor processor = new(game);

            // Act
            processor.NextPlayer();

            // Assert
            Assert.That(game.Players.Peek().Username, Is.EqualTo("Player 2"));
        }

        [Test]
        public void TestReversePlayers()
        {
            // Arrange
            Game game = new("testpassword", 4);
            game.Players.Enqueue(new Player("Player 1"));
            game.Players.Enqueue(new Player("Player 2"));
            game.Players.Enqueue(new Player("Player 3"));
            game.Players.Enqueue(new Player("Player 4"));
            Processor processor = new(game);

            // Act
            processor.ReversePlayers();
            List<Player> players = new(game.Players);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(players[0].Username, Is.EqualTo("Player 4"));
                Assert.That(players[1].Username, Is.EqualTo("Player 3"));
                Assert.That(players[2].Username, Is.EqualTo("Player 2"));
                Assert.That(players[3].Username, Is.EqualTo("Player 1"));
            });
        }

        [Test]
        public void TestShufflePlayers()
        {
            // Arrange
            Game game = new("testpassword", 4);
            game.Players.Enqueue(new Player("Player 1"));
            game.Players.Enqueue(new Player("Player 2"));
            game.Players.Enqueue(new Player("Player 3"));
            game.Players.Enqueue(new Player("Player 4"));
            Processor processor = new(game);

            // Act
            processor.ShufflePlayers();

            // Assert
            List<Player> players = new(game.Players);
            Assert.That(players, Has.Count.EqualTo(4));
        }

        [Test]
        public void TestSetup()
        {
            // Arrange
            Game game = new("testpassword", 4);
            game.Players.Enqueue(new Player("Player 1"));
            game.Players.Enqueue(new Player("Player 2"));
            game.Players.Enqueue(new Player("Player 3"));
            game.Players.Enqueue(new Player("Player 4"));
            Processor processor = new(game);

            // Act
            processor.Setup();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(game.DrawPile, Has.Count.EqualTo(79));
                Assert.That(game.DiscardPile, Has.Count.EqualTo(1));
                Assert.That(game.Players.Peek().Cards, Has.Count.EqualTo(7));
                Assert.That(game.Running, Is.True);
            });
        }

        private static object[] _testValidCases =
        {
            new object[]
            {
                new Card(CardColor.Red, CardValue.C0),
                new Move(new Card(CardColor.Blue, CardValue.C0), MoveAction.Play),
                0, true
            },
            new object[]
            {
                new Card(CardColor.Red, CardValue.C0),
                new Move(new Card(CardColor.Red, CardValue.C3), MoveAction.Play),
                0, true
            },
            new object[]
            {
                new Card(CardColor.Special, CardValue.Draw4),
                new Move(new Card(CardColor.Blue, CardValue.C3), MoveAction.Play),
                2, false
            },
            new object[]
            {
                new Card(CardColor.Red, CardValue.Draw2),
                new Move(new Card(CardColor.Blue, CardValue.C3), MoveAction.Play),
                4, false
            },
            new object[]
            {
                new Card(CardColor.Blue, CardValue.Wild),
                new Move(new Card(CardColor.Blue, CardValue.C3), MoveAction.Play),
                0, true
            },
            new object[]
            {
                new Card(CardColor.Red, CardValue.Wild),
                new Move(new Card(CardColor.Blue, CardValue.C3), MoveAction.Play),
                0, false
            },
            new object[]
            {
                new Card(CardColor.Red, CardValue.Draw2),
                new Move(new Card(CardColor.Blue, CardValue.C3), MoveAction.Play),
                4, false
            },
            new object[]
            {
                new Card(CardColor.Red, CardValue.C0),
                new Move(null, MoveAction.Draw),
                0, true
            },
            new object[]
            {
                new Card(CardColor.Red, CardValue.C0),
                new Move(null, MoveAction.Draw),
                6, true
            }
        };

        [Test]
        [TestCaseSource(nameof(_testValidCases))]
        public void TestValid(Card topCard, Move move, int drawBuffer, bool expected)
        {
            // Arrange
            Game game = new("testpassword", 4);
            Player player1 = new("Player 1");
            Processor processor = new(game);
            game.DiscardPile.Push(topCard);
            game.DrawBuffer = drawBuffer;

            if (move.Action == MoveAction.Play)
            {
                player1.Cards.Add(move.Card!);
            }

            // Act
            bool valid = processor.Valid(player1, move);

            // Assert
            Assert.That(valid, Is.EqualTo(expected));
        }
    }
}