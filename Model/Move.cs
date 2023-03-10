namespace Model;

public class Move
{
    #region Properties

    public MoveAction Action { get; }
    public Card? Card { get; }

    #endregion

    #region Constructor

    public Move(Card? card, MoveAction action)
    {
        Card = card;
        Action = action;
    }

    #endregion
}