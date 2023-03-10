namespace Model;

public static class Deck
{
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

    public static Queue<Card> Shuffle(Queue<Card> deck)
    {
        return new Queue<Card>(deck.OrderBy(_ => new Random().Next()));
    }
}