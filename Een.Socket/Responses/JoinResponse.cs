using Een.Model;

namespace Een.Socket.Responses;

public class JoinResponse : SocketResponse
{
    #region Properties

    public Queue<Player> Players { get; }

    #endregion

    #region Constructor

    public JoinResponse(bool success, string message, Queue<Player> players) : base(success, message)
    {
        Players = players;
    }

    #endregion
}