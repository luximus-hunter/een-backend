using Api.Model;
using Microsoft.AspNetCore.SignalR;
using Model;
using Newtonsoft.Json;

namespace Api.Hubs;

public class GamesHub : Hub
{
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
    /// <returns><see cref="ISingleClientProxy"/></returns>
    private ISingleClientProxy Sender => Clients.Client(Context.ConnectionId);

    #endregion

    #region Public Methods

    /// <summary>
    /// Processes a <see cref="Move"/> being made.
    /// </summary>
    /// <param name="data">Serialised JSON version of <see cref="MoveRequest"/>.</param>
    public async Task Move(string data)
    {
        MoveRequest? r = JsonConvert.DeserializeObject<MoveRequest>(data);

        if (r == null)
        {
            await Sender.SendAsync("Err", new MoveResponse(false, "The send request is invalid").ToString());
            return;
        }

        Game? game = GamesManager.Find(r.GameId);

        if (!GamesManager.Exists(r.GameId) || game == null)
        {
            await Sender.SendAsync("Err",
                new MoveResponse(false, "The game you're looking for doesn't exist").ToString());
            return;
        }

        if (!game.HasPlayer(r.PlayerId))
        {
            await Sender.SendAsync("Err",
                new MoveResponse(false, "The player ID you entered doesn't exist in the game you're looking in")
                    .ToString());
            return;
        }

        Player player = game.GetPlayer(r.PlayerId);

        if (game.Processor.Valid(player, r.Move))
        {
            game.Processor.Execute(player, r.Move);

            if (game.Players.Any(p => p.Cards.Count < 1))
            {
                await Group(r.GameId).SendAsync("Win",
                    new WinResponse(true, "Success", game.Players.First(p => p.Cards.Count < 1)).ToString());
            }
        }
        else
        {
            await Sender.SendAsync("Err", new MoveResponse(false, "The move you played was invalid").ToString());
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
            await Sender.SendAsync("Err", new SocketResponse(false, "The send request is invalid").ToString());
            return;
        }

        Game? game = GamesManager.Find(r.GameId);

        // Check if the game exists
        if (game == null)
        {
            await Sender.SendAsync("Err",
                new SocketResponse(false, "The game you're looking for doesn't exist").ToString());
            return;
        }

        // Check if the player is in the game
        if (!game.HasPlayer(r.PlayerId))
        {
            await Sender.SendAsync("Err",
                new SocketResponse(false, "The player ID you entered doesn't exist in the game you're looking in")
                    .ToString());
            return;
        }

        // Prevent duplicates
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, r.GameId.ToString());
        await Groups.AddToGroupAsync(Context.ConnectionId, r.GameId.ToString());

        await Group(game.Id)
            .SendAsync("Join",
                new JoinResponse(true, $"{r.PlayerId} has joined the group {game.Id}.", game.Players).ToString());
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
            await Sender.SendAsync("Err", new SocketResponse(false, "The send request is invalid").ToString());
            return;
        }

        Game? game = GamesManager.Find(request.GameId);

        if (game == null)
        {
            await Sender.SendAsync("Err",
                new SocketResponse(false,
                    "The game you're looking for doesn't existThe game you're looking for doesn't exist").ToString());
            return;
        }

        game.Start();

        await Group(game.Id).SendAsync("Start", new MoveResponse(true, $"Game {game.Id} has started", game).ToString());
    }

    #endregion
}