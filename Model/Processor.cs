namespace Model;

public class Processor
{
    private readonly Game _game;

    public Processor(Game game)
    {
        _game = game;
    }

    public bool ValidateMove(Guid playerId, Move move)
    {
        bool valid;

        Player player = _game.Players.First(p => p.Id == playerId);
        Card lastPlayed = _game.DiscardPile.Peek();

        //TODO: Make this cleaner
        switch (move)
        {
            // Player draws a card
            case { Action: MoveAction.Draw, Card: null }:
                Console.WriteLine("Player draws");
                valid = true;
                break;
            // Player plays a card
            case { Action: MoveAction.Play, Card: { } }:
            {
                Console.WriteLine("Player plays");
                Console.WriteLine($"OLD: Color: {lastPlayed.Color}, Value: {lastPlayed.Value}");
                Console.WriteLine($"NEW: Color: {move.Card.Color}, Value: {move.Card.Value}");

                // Player has card
                if (CheckPlayersHand(player, move.Card))
                {
                    Console.WriteLine("Player has card");

                    // If player needs to draw a card
                    if (_game.DrawBuffer > 0)
                    {
                        Console.WriteLine("There is a buffer");

                        if (lastPlayed.Value == CardValue.Draw4)
                        {
                            Console.WriteLine("Last played card was a +4");
                            // +2 not on +4
                            valid = move.Card.Value == CardValue.Draw4;

                            // +2 on +4 allowed
                            // valid = move.Card.Value == CardValue.Draw4 || move.Card.Value == CardValue.Draw2;
                        }
                        else if (lastPlayed.Value == CardValue.Draw2)
                        {
                            Console.WriteLine("Last played card was a +2");

                            valid = move.Card.Value == CardValue.Draw4 || move.Card.Value == CardValue.Draw2;
                        }
                        else
                        {
                            Console.WriteLine("Player cant play and needs to draw");
                            valid = false;
                        }
                    }
                    // Special cards
                    // And wild cuz wild can be all colors
                    else if (move.Card.Value == CardValue.Wild || move.Card.Color == CardColor.Special)
                    {
                        Console.WriteLine("Player played Wild or a special");

                        valid = true;
                    }
                    // Last played card was special
                    else if (lastPlayed.Color == CardColor.Special)
                    {
                        Console.WriteLine("Last card was a special so anything is fine");

                        valid = true;
                    }
                    // Normal cards
                    else if (lastPlayed.Color == move.Card.Color || lastPlayed.Value == move.Card.Value)
                    {
                        Console.WriteLine("Player played a matching card");

                        valid = true;
                    }
                    // Invalid card
                    else
                    {
                        Console.WriteLine("Not a valid card to play");

                        valid = false;
                    }
                }
                // Player doesnt have card
                else
                {
                    Console.WriteLine("Player doesnt have the card they say they have");

                    valid = false;
                }

                break;
            }
            // Invalid action
            default:
                Console.WriteLine("Player did an invalid action");

                valid = false;
                break;
        }

        Console.WriteLine($"Valid: {valid}");

        return valid;
    }

    private static bool CheckPlayersHand(Player player, Card card)
    {
        Console.WriteLine("Checking players hand");
        Console.WriteLine("Player has:");

        foreach (Card c in player.Cards)
        {
            Console.WriteLine($"Color: {c.Color}, Value: {c.Value}");
        }

        if (card.Value == CardValue.Wild)
        {
            return player.Cards.Any(c => c.Value == card.Value);
        }

        return player.Cards.Any(c => c.Value == card.Value && c.Color == card.Color);
    }

    public void ExecuteMove(Guid playerId, Move move)
    {
        Player player = _game.Players.First(p => p.Id == playerId);

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

                _game.NextPlayer();
                _game.OnTurn = _game.Players.Peek().Id;
                break;
            // Player plays a card
            case { Action: MoveAction.Play, Card: { } }:
                Card lastPlayed = _game.DiscardPile.Peek();

                if (move.Card.Color == lastPlayed.Color || move.Card.Value == lastPlayed.Value ||
                    move.Card.Value == CardValue.Wild || move.Card.Color == CardColor.Special ||
                    lastPlayed.Color == CardColor.Special)
                {
                    if (move.Card.Value == CardValue.Wild)
                    {
                        player.RemoveCard(move.Card.Value);
                    }
                    else
                    {
                        player.RemoveCard(move.Card);
                    }

                    _game.DiscardPile.Push(move.Card);

                    switch (move.Card.Value)
                    {
                        case CardValue.Reverse:
                            _game.ReversePlayers();
                            if (_game.Players.Count == 2)
                            {
                                _game.NextPlayer();
                            }

                            break;
                        case CardValue.Skip:
                            _game.NextPlayer();
                            _game.NextPlayer();
                            break;
                        case CardValue.Draw4:
                            _game.DrawBuffer += 4;
                            _game.NextPlayer();
                            break;
                        case CardValue.Draw2:
                            _game.DrawBuffer += 2;
                            _game.NextPlayer();
                            break;
                        case CardValue.Wild:
                            _game.NextPlayer();
                            break;
                        default:
                            _game.NextPlayer();
                            break;
                    }

                    _game.OnTurn = _game.Players.Peek().Id;
                }

                break;
        }
    }

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
                player.Cards.Add(_game.DrawPile.Dequeue());
            }
        }

        // Place first card in discard pile
        _game.DiscardPile.Push(_game.DrawPile.Dequeue());

        // Randomise player order
        _game.ShufflePlayers();
        _game.OnTurn = _game.Players.Peek().Id;
    }

    public void RestockDrawPile()
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
    /// 
    /// </summary>
    /// <param name="player"><see cref="Player"/> that the cards will be added to.</param>
    /// <param name="amount">Amount of cards that need to be drawn.</param>
    public void DrawCards(Player player, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            player.Cards.Add(_game.DrawPile.Dequeue());

            if (_game.DrawPile.Count < 1)
            {
                RestockDrawPile();
            }
        }
    }
}