using Newtonsoft.Json;

namespace Een.Socket.Requests;

public class GameRequest
{
    #region Properties

    public Guid GameId { get; }

    #endregion

    #region Constructor

    public GameRequest(Guid gameId)
    {
        GameId = gameId;
    }

    #endregion

    #region Public Methods

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    #endregion
}