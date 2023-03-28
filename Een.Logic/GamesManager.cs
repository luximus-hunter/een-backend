using Een.Model;

namespace Een.Logic;

public static class GamesManager
{
    #region Fields

    public static Dictionary<Guid, Game> Games { get; } = new();
    public static Dictionary<Guid, Processor> Processors { get; } = new();

    #endregion

    #region Properties

    public const int MaxPlayers = 4;

    #endregion

    #region Public Methods

    /// <summary>
    /// Creates a game with the provided properties.
    /// </summary>
    /// <param name="password">Password of the game to join.</param>
    /// <param name="maxPlayers">The maximum amount of players for a game. Can be 2, 3 or 4.</param>
    /// <returns></returns>
    public static Game New(string password, int maxPlayers)
    {
        Game game = new(password, maxPlayers);

        Games.Add(game.Id, game);
        Processors.Add(game.Id, new Processor(game));

        return game;
    }

    /// <summary>
    /// Checks if a game exists based on a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> of game to check.</param>
    /// <returns>True if game exists, False if game doesn't exist</returns>
    public static bool Exists(Guid id) => Games.ContainsKey(id);

    /// <summary>
    /// Looks for a game based on a <see cref="Game"/> <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> of game to check.</param>
    /// <returns>Returns the found <see cref="Game"/> or null if it's not found.</returns>
    public static Game? Find(Guid id) => Exists(id) ? Games[id] : null;

    /// <summary>
    /// Looks for a game based on a <see cref="Player"/> <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> of the player to check</param>
    /// <returns>Returns the found <see cref="Game"/> or null if it's not found.</returns>
    public static Game? FindByPlayerId(Guid id) =>
        Games.Select(entry => entry.Value).FirstOrDefault(g => g.Players.Any(p => p.Id == id));

    /// <summary>
    /// Gets the <see cref="Processor"/> for a <see cref="Game"/>
    /// </summary>
    /// <param name="id"><see cref="Guid"/> of <see cref="Game"/> to find who's <see cref="Processor"/></param>
    /// <returns><see cref="Processor"/> of game</returns>
    public static Processor Processor(Guid id) => Processors[id];

    /// <summary>
    /// Deletes all games. Used for testing
    /// </summary>
    public static void Empty()
    {
        foreach ((Guid id, Game? _) in Games)
        {
            Games.Remove(id);
            Processors.Remove(id);
        }
    }

    /// <summary>
    /// Deletes all games without players.
    /// </summary>
    public static void Purge()
    {
        int count = 0;

        foreach ((Guid id, Game? game) in Games)
        {
            if (game.Players.Count < 1)
            {
                Games.Remove(id);
                Processors.Remove(id);
                count++;
            }
        }

        if (count > 0)
        {
            Console.WriteLine($"info: Purged {count} games");
        }
    }

    #endregion
}