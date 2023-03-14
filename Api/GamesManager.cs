using Model;

namespace Api;

public static class GamesManager
{
    #region Fields

    private static Dictionary<Guid, Game> Games { get; } = new();

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