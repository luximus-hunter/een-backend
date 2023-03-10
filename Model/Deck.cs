namespace Model;

public static class Deck
{
    #region Public Methods

    /// <summary>
    /// Creates a new <see cref="Queue"/> of <see cref="Card"/> with all cards in the game
    /// </summary>
    /// <returns>
    /// <see cref="Queue"/> Where T is <see cref="Card"/>
    /// </returns>
    public static Queue<Card> New()
    {
        Queue<Card> deck = new();

        Array colors = Enum.GetValues(typeof(CardColor));
        Array values = Enum.GetValues(typeof(CardValue));

        foreach (CardColor color in colors)
        {
            if (color == CardColor.Special)
            {
                for (int i = 0; i < 4; i++)
                {
                    deck.Enqueue(new Card(CardColor.Special, CardValue.Wild));
                    deck.Enqueue(new Card(CardColor.Special, CardValue.Draw4));
                }
            }
            else
            {
                foreach (CardValue value in values)
                {
                    switch (value)
                    {
                        case CardValue.C0:
                            deck.Enqueue(new Card(color, value));
                            break;
                        case CardValue.Wild:
                        case CardValue.Draw4:
                            // Dont add these, they're done above
                            break;
                        default:
                            deck.Enqueue(new Card(color, value));
                            deck.Enqueue(new Card(color, value));
                            break;
                    }
                }
            }
        }

        return Shuffle(deck);
    }

    /// <summary>
    /// Shuffles a <see cref="Queue"/> of <see cref="Card"/>
    /// </summary>
    /// <returns>
    /// <see cref="Queue"/> Where T is <see cref="Card"/>
    /// </returns>
    public static Queue<Card> Shuffle(Queue<Card> deck)
    {
        return new Queue<Card>(deck.OrderBy(_ => new Random().Next()));
    }

    #endregion
}