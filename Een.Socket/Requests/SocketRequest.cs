namespace Een.Socket.Requests;

public class SocketRequest : GameRequest
{
    #region Properties

    public Guid PlayerId { get; }

    #endregion

    #region Constructor

    public SocketRequest(Guid gameId, Guid playerId) : base(gameId)
    {
        PlayerId = playerId;
    }

    #endregion
}