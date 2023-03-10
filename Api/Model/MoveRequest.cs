using Model;

namespace Api.Model;

public class MoveRequest : SocketRequest
{
    public Move Move { get; }

    public MoveRequest(Guid gameId, Guid playerId, Move move) : base(gameId, playerId)
    {
        Move = move;
    }
}