namespace Api.Model;

public class JoinRequest : GameRequest
{
    public Guid PlayerId { get; }

    public JoinRequest(Guid gameId, Guid playerId) : base(gameId)
    {
        PlayerId = playerId;
    }
}