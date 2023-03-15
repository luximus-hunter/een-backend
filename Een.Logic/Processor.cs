using Een.Model;

namespace Een.Logic;

public class Processor
{
    #region Fields

    private readonly Game _game;

    #endregion

    #region Constructor

    /// <summary>
    /// Sets the <see cref="Game"/> to process
    /// </summary>
    /// <param name="game"><see cref="Game"/> to process</param>
    public Processor(Game game)
    {
        _game = game;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Places all cards from the discardpile, except the top card, back into the drawpile and shuffles them
    /// </summary>
    private void RestockDrawPile()
    {
        Card top = _game.DiscardPile.Pop();

        while (_game.DiscardPile.Count > 0)
        {
            Card card = _game.DiscardPile.Pop();

            if (card.Value == CardValue.Wild)
            {
                card.Color = CardColor.Special;
            }

            _game.DrawPile.Enqueue(card);
        }

        _game.DrawPile = Deck.Shuffle(_game.DrawPile);
        _game.DiscardPile.Push(top);
    }

    /// <summary>
    /// Adds the specified amount of cards to the specified player.
    /// </summary>
    /// <param name="player"><see cref="Player"/> that the cards will be added to.</param>
    /// <param name="amount">Amount of cards that need to be drawn.</param>
    private void DrawCards(Player player, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            player.AddCard(_game.DrawPile.Dequeue());

            if (_game.DrawPile.Count < 1)
            {
                RestockDrawPile();
            }
        }
    }

    /// <summary>
    /// Moves the current player to the last space in the player list.
    /// </summary>
    private void NextPlayer()
    {
        _game.Players.Enqueue(_game.Players.Dequeue());
    }

    /// <summary>
    /// Reverses the player list.
    /// </summary>
    private void ReversePlayers()
    {
        _game.Players = new Queue<Player>(_game.Players.Reverse());
    }

    /// <summary>
    /// Randomizes the player order.
    /// </summary>
    private void ShufflePlayers()
    {
        _game.Players = new Queue<Player>(_game.Players.OrderBy(_ => new Random().Next()));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Validates a <see cref="Move"/> done by the specified <see cref="Player"/>
    /// </summary>
    /// <param name="player"><see cref="Player"/> that makes the <see cref="Move"/></param>
    /// <param name="move"><see cref="Move"/> that needs to be validated</param>
    /// <returns><see cref="Boolean"/></returns>
    //TODO: Make this cleaner
    public bool Valid(Player player, Move move)
    {
        bool valid;

        Card lastPlayed = _game.DiscardPile.Peek();

        switch (move)
        {
            // Player draws a card
            case { Action: MoveAction.Draw, Card: null }:
                valid = true;
                break;
            // Player plays a card
            case { Action: MoveAction.Play, Card: { } }:
            {
                // Player has card
                if (player.HasCard(move.Card))
                {
                    // If player needs to draw a card
                    if (_game.DrawBuffer > 0)
                    {
                        if (lastPlayed.Value == CardValue.Draw4)
                        {
                            // +2 not on +4
                            valid = move.Card.Value == CardValue.Draw4;

                            // +2 on +4 allowed
                            // valid = move.Card.Value == CardValue.Draw4 || move.Card.Value == CardValue.Draw2;
                        }
                        else if (lastPlayed.Value == CardValue.Draw2)
                        {
                            valid = move.Card.Value == CardValue.Draw4 || move.Card.Value == CardValue.Draw2;
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                    // Special cards
                    // And wild cuz wild can be all colors
                    else if (move.Card.Value == CardValue.Wild || move.Card.Color == CardColor.Special)
                    {
                        valid = true;
                    }
                    // Last played card was special
                    else if (lastPlayed.Color == CardColor.Special)
                    {
                        valid = true;
                    }
                    // Normal cards
                    else if (lastPlayed.Color == move.Card.Color || lastPlayed.Value == move.Card.Value)
                    {
                        valid = true;
                    }
                    // Invalid card
                    else
                    {
                        valid = false;
                    }
                }
                // Player doesnt have card
                else
                {
                    valid = false;
                }

                break;
            }
            // Invalid action
            default:
                valid = false;
                break;
        }

        Console.WriteLine($"Valid: {valid}");

        return valid;
    }

    /// <summary>
    /// Processes a <see cref="Move"/> done by the specified <see cref="Player"/>
    /// </summary>
    /// <param name="player"><see cref="Player"/> that makes the <see cref="Move"/></param>
    /// <param name="move"><see cref="Move"/> that needs to be executed</param>
    public void Execute(Player player, Move move)
    {
        switch (move)
        {
            // Player draws a card
            case { Action: MoveAction.Draw, Card: null }:
                if (_game.DrawBuffer > 0)
                {
                    DrawCards(player, _game.DrawBuffer);
                    _game.DrawBuffer = 0;
                }
                else
                {
                    DrawCards(player, 1);
                }

                NextPlayer();
                _game.OnTurn = _game.Players.Peek().Id;
                break;
            // Player plays a card
            case { Action: MoveAction.Play, Card: { } }:
                Card lastPlayed = _game.DiscardPile.Peek();

                if (move.Card.Color == lastPlayed.Color || move.Card.Value == lastPlayed.Value ||
                    move.Card.Value == CardValue.Wild || move.Card.Color == CardColor.Special ||
                    lastPlayed.Color == CardColor.Special)
                {
                    player.RemoveCard(move.Card);
                    _game.DiscardPile.Push(move.Card);

                    switch (move.Card.Value)
                    {
                        case CardValue.Reverse:
                            ReversePlayers();
                            if (_game.Players.Count == 2)
                            {
                                NextPlayer();
                            }

                            break;
                        case CardValue.Skip:
                            NextPlayer();
                            NextPlayer();
                            break;
                        case CardValue.Draw4:
                            _game.DrawBuffer += 4;
                            NextPlayer();
                            break;
                        case CardValue.Draw2:
                            _game.DrawBuffer += 2;
                            NextPlayer();
                            break;
                        case CardValue.Wild:
                            NextPlayer();
                            break;
                        default:
                            NextPlayer();
                            break;
                    }

                    _game.OnTurn = _game.Players.Peek().Id;
                }

                break;
        }
    }

    /// <summary>
    /// Set up all players and piles.
    /// </summary>
    public void Setup()
    {
        // Reset piles
        _game.DrawPile = Deck.New();
        _game.DiscardPile = new Stack<Card>();
        _game.DrawBuffer = 0;

        // Give each player 7 cards
        foreach (Player player in _game.Players)
        {
            player.Cards = new List<Card>();
            while (player.Cards.Count < 7)
            {
                player.AddCard(_game.DrawPile.Dequeue());
            }
        }

        // Place first card in discard pile
        _game.DiscardPile.Push(_game.DrawPile.Dequeue());

        // Randomise player order
        ShufflePlayers();
        _game.OnTurn = _game.Players.Peek().Id;

        _game.Running = true;
    }

    #endregion
}