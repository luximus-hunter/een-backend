namespace Api.Model;

public class SocketRequest : GameRequest
{
    public Guid PlayerId { get; set; }

    public SocketRequest(Guid gameId, Guid playerId) : base(gameId)
    {
        PlayerId = playerId;
    }
}