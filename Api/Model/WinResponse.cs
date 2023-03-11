using Model;

namespace Api.Model;

public class WinResponse : SocketResponse
{
    #region Properties

    public Player Player { get; }

    #endregion

    #region Constructor

    public WinResponse(bool success, string message, Player player) : base(success, message)
    {
        Player = player;
    }

    #endregion
}