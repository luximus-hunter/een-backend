using Model;

namespace Api.Model;

public class JoinResponse : SocketResponse
{
    public Queue<Player> Players { get; }

    public JoinResponse(bool success, string message, Queue<Player> players) : base(success, message)
    {
        Players = players;
    }
}