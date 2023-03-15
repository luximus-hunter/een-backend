using Een.Model;

namespace Een.Socket.Requests;

public class MoveRequest : SocketRequest
{
    #region Properties

    public Move Move { get; }

    #endregion

    #region Constructor

    public MoveRequest(Guid gameId, Guid playerId, Move move) : base(gameId, playerId)
    {
        Move = move;
    }

    #endregion
}