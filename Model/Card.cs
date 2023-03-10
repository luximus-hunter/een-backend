namespace Model;

public class Card
{
    public CardColor Color { get; set; }
    public CardValue Value { get; }

    public Card(CardColor color, CardValue value)
    {
        Color = color;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Color} card with value {Value}";
    }
}