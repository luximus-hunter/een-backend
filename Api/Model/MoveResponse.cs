using Model;

namespace Api.Model;

public class MoveResponse : SocketResponse
{
    public Game? Game { get; }

    public MoveResponse(bool success, string message) : base(success, message)
    {
    }

    public MoveResponse(bool success, string message, Game? game = null) : base(success, message)
    {
        Game = game;
    }
}