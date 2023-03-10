using Model;

namespace Api;

public static class GamesManager
{
    public const int MaxPlayers = 4;
    private static Dictionary<Guid, Game> Games { get; } = new();

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
    /// Looks for a game based on a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> of game to check.</param>
    /// <returns>Returns the found game or null if it's not found.</returns>
    public static Game? Find(Guid id) => Exists(id) ? Games[id] : null;
}