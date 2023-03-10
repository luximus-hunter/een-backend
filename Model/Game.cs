using static BCrypt.Net.BCrypt;

namespace Model;

public class Game
{
    public bool Running { get; set; }
    public Guid Id { get; }
    public Guid OnTurn { get; set; }
    public string Password { get; }
    public int MaxPlayers { get; }
    public Queue<Player> Players { get; set; }
    public Queue<Card> DrawPile { get; set; }
    public Stack<Card> DiscardPile { get; set; }
    public int DrawBuffer { get; set; }

    public readonly Processor Processor;

    public Game(Guid id, string password, int maxPlayers) : this(password, maxPlayers)
    {
        Id = id;
    }

    public Game(string password, int maxPlayers)
    {
        Id = Guid.NewGuid();
        Password = HashPassword(password);
        MaxPlayers = maxPlayers;
        Players = new Queue<Player>(MaxPlayers);
        DrawBuffer = 0;
        Processor = new Processor(this);
    }

    public bool CheckPassword(string password)
    {
        return Verify(password, Password);
    }

    public void Start()
    {
        Running = true;
        Processor.Setup();
    }

    public void NextPlayer()
    {
        Players.Enqueue(Players.Dequeue());
    }

    public void ReversePlayers()
    {
        Players = new Queue<Player>(Players.Reverse());
    }

    public void ShufflePlayers()
    {
        Players = new Queue<Player>(Players.OrderBy(_ => new Random().Next()));
    }

    public bool HasPlayer(Guid id)
    {
        return Players.Any(player => player.Id == id);
    }

    public Player GetPlayer(Guid id)
    {
        return Players.First(player => player.Id == id);
    }
}