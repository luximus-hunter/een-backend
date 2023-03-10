using Model;

namespace Api.Model;

public class MoveResponse : SocketResponse
{
    #region Properties

    public Game? Game { get; }

    #endregion

    #region Constructor

    public MoveResponse(bool success, string message, Game? game = null) : base(success, message)
    {
        Game = game;
    }

    #endregion
}