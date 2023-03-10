using Microsoft.AspNetCore.Mvc;
using Model;

namespace Api.Controllers;

[ApiController]
[Route("games")]
public class GamesController : ControllerBase
{
    #region Fields

    private readonly ILogger<GamesController> _logger;

    #endregion

    #region Constructor

    public GamesController(ILogger<GamesController> logger)
    {
        _logger = logger;
        _logger.Log(LogLevel.Information, @"Games route created");
    }

    #endregion

    #region Private Methods

    private static string ErrorMessage(string message) => $"{{\"status\": false, \"message\": \"{message}\"}}";

    #endregion

    #region Public Methods

    /// <summary>
    /// Creates a game with the provided properties. Also adds the player that created it.
    /// </summary>
    /// <param name="password">Password of the game to join.</param>
    /// <param name="maxPlayers">The maximum amount of players for a game. Can be 2, 3 or 4.</param>
    /// <param name="username">Username to display the player as.</param>
    /// <returns>
    /// <see cref="IActionResult"/> 
    /// <seealso cref="OkObjectResult"/>
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    public IActionResult Create(string password, int maxPlayers, string username)
    {
        maxPlayers = Math.Clamp(maxPlayers, 2, GamesManager.MaxPlayers);

        Game game = GamesManager.New(password, maxPlayers);

        Player player = new(username);
        game.Players.Enqueue(player);

        return Ok(game);
    }

    /// <summary>
    /// Adds a new player to a game if they provide the correct password.
    /// </summary>
    /// <param name="gameId">ID of the game to join.</param>
    /// <param name="password">Password of the game to join.</param>
    /// <param name="username">Username to display the player as.</param>
    /// <returns>
    /// <see cref="IActionResult"/>
    /// <seealso cref="OkObjectResult"/>
    /// <seealso cref="NotFoundObjectResult"/>
    /// <seealso cref="UnauthorizedObjectResult"/>
    /// </returns>
    [HttpPost("join")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Player))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Join(Guid gameId, string password, string username)
    {
        Game? game = GamesManager.Find(gameId);

        if (game == null) return NotFound(ErrorMessage("Game not found."));
        if (!game.CheckPassword(password)) return Unauthorized(ErrorMessage("Password incorrect."));
        if (game.Running) return Unauthorized(ErrorMessage("Game is already active."));
        if (game.Players.Count >= game.MaxPlayers) return Unauthorized(ErrorMessage("Game is full."));

        Player player = new(username);

        game.Players.Enqueue(player);
        return Ok(player);
    }

    #endregion
}