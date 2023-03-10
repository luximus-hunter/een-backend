using Newtonsoft.Json;

namespace Api.Model;

public class GameRequest
{
    public Guid GameId { get; }

    public GameRequest(Guid gameId)
    {
        GameId = gameId;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}