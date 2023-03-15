using Een.Logic;
using Een.Model;
using Een.Socket.Requests;
using Een.Socket.Responses;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Een.Socket;

public class GamesHub : Hub
{
    #region Fields

    private static Dictionary<string, Guid> _players = new();

    #endregion

    #region Private Methods

    /// <summary>
    /// Returns the current group based on a Guid.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> of the group.</param>
    /// <returns><see cref="IClientProxy"/></returns>
    private IClientProxy Group(Guid id) => Clients.Group(id.ToString());

    /// <summary>
    /// Returns the sender of the WebSocket request.
    /// </summary>
    /// <returns><see cref="IClientProxy"/></returns>
    private IClientProxy Sender => Clients.Client(Context.ConnectionId);

    #endregion

    #region Public Methods

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string connection = Context.ConnectionId;
        Guid playerId = _players[connection];
        Game? game = GamesManager.FindByPlayerId(playerId);

        if (game == null) return null;
        
        game.RemovePlayer(playerId);
        
        GamesManager.Purge();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Processes a <see cref="Move"/> being made.
    /// </summary>
    /// <param name="data">Serialised JSON version of <see cref="MoveRequest"/>.</param>
    public async Task Move(string data)
    {
        MoveRequest? r = JsonConvert.DeserializeObject<MoveRequest>(data);

        if (r == null)
        {
            await Sender.SendAsync("Err", new MoveResponse(false, "The send request is invalid.").ToString());
            return;
        }

        Game? game = GamesManager.Find(r.GameId);

        if (!GamesManager.Exists(r.GameId) || game == null)
        {
            await Sender.SendAsync("Err",
                new MoveResponse(false, "The game you're looking for doesn't exist.").ToString());
            return;
        }

        if (!game.HasPlayer(r.PlayerId))
        {
            await Sender.SendAsync("Err",
                new MoveResponse(false, "The player ID you entered doesn't exist in the game you're looking in.")
                    .ToString());
            return;
        }

        Player player = game.GetPlayer(r.PlayerId);

        if (_players.TryGetValue(Context.ConnectionId, out Guid value) && value != player.Id)
        {
            await Sender.SendAsync("Err",
                new MoveResponse(false, "You committed identity fraud.")
                    .ToString());
            return;
        }

        if (GamesManager.Processor(game.Id).Valid(player, r.Move))
        {
            GamesManager.Processor(game.Id).Execute(player, r.Move);

            if (game.Players.Any(p => p.Cards.Count < 1))
            {
                game.Running = false;
                await Group(r.GameId).SendAsync("Win",
                    new WinResponse(true, "Success", game.Players.First(p => p.Cards.Count < 1)).ToString());
            }
        }
        else
        {
            await Sender.SendAsync("Err", new MoveResponse(false, "The move you played was invalid.").ToString());
            return;
        }

        await Group(r.GameId).SendAsync("Move", new MoveResponse(true, "Success", game).ToString());
    }

    /// <summary> 
    /// Processes a <see cref="Player"/> joining a <see cref="Game"/>.
    /// </summary>
    /// <param name="data">Serialised JSON version of <see cref="JoinRequest"/>.</param>
    public async Task Join(string data)
    {
        // Convert json to object
        JoinRequest? r = JsonConvert.DeserializeObject<JoinRequest>(data);

        // Check if object is valid
        if (r == null)
        {
            await Sender.SendAsync("Err", new SocketResponse(false, "The send request is invalid.").ToString());
            return;
        }

        Game? game = GamesManager.Find(r.GameId);

        // Check if the game exists
        if (game == null)
        {
            await Sender.SendAsync("Err",
                new SocketResponse(false, "The game you're looking for doesn't exist.").ToString());
            return;
        }

        // Check if the player is in the game
        if (!game.HasPlayer(r.PlayerId))
        {
            await Sender.SendAsync("Err",
                new SocketResponse(false, "The player ID you entered doesn't exist in the game you're looking in.")
                    .ToString());
            return;
        }

        Player player = game.GetPlayer(r.PlayerId);

        // Link player to connection id
        _players.Add(Context.ConnectionId, player.Id);

        // Prevent duplicates
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, r.GameId.ToString());
        await Groups.AddToGroupAsync(Context.ConnectionId, r.GameId.ToString());

        await Group(game.Id)
            .SendAsync("Join",
                new JoinResponse(true, $"{player.Id} has joined group {game.Id}.", game.Players).ToString());
    }

    /// <summary>
    /// Processes a <see cref="Game"/> being started.
    /// </summary>
    /// <param name="data">Serialised JSON version of <see cref="GameRequest"/>.</param>
    public async Task Start(string data)
    {
        GameRequest? request = JsonConvert.DeserializeObject<GameRequest>(data);

        if (request == null)
        {
            await Sender.SendAsync("Err", new SocketResponse(false, "The send request is invalid.").ToString());
            return;
        }

        Game? game = GamesManager.Find(request.GameId);

        if (game == null)
        {
            await Sender.SendAsync("Err",
                new SocketResponse(false,
                    "The game you're looking for doesn't existThe game you're looking for doesn't exist.").ToString());
            return;
        }

        GamesManager.Processor(game.Id).Setup();

        await Group(game.Id)
            .SendAsync("Start", new MoveResponse(true, $"Game {game.Id} has started.", game).ToString());
    }

    #endregion
}