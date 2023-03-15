namespace Een.Model;

public class Card
{
    #region Properties

    public CardColor Color { get; set; }
    public CardValue Value { get; }

    #endregion

    #region Constructor

    public Card(CardColor color, CardValue value)
    {
        Color = color;
        Value = value;
    }

    #endregion
}