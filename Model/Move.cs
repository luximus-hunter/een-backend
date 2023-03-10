namespace Model;

public class Move
{
    public MoveAction Action { get; }
    public Card? Card { get; }

    public Move(Card? card, MoveAction action)
    {
        Card = card;
        Action = action;
    }
}