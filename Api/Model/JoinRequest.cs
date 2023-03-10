namespace Api.Model;

public class JoinRequest : GameRequest
{
    #region Properties

    public Guid PlayerId { get; }

    #endregion

    #region Constructor

    public JoinRequest(Guid gameId, Guid playerId) : base(gameId)
    {
        PlayerId = playerId;
    }

    #endregion
}