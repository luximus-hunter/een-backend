using static BCrypt.Net.BCrypt;

namespace Model;

public class Game
{
    #region Properties

    public Guid Id { get; }
    public bool Running { get; set; }
    public Guid OnTurn { get; set; }
    public string Password { get; }
    public int MaxPlayers { get; }
    public Queue<Player> Players { get; set; }
    public Queue<Card> DrawPile { get; set; }
    public Stack<Card> DiscardPile { get; set; }
    public int DrawBuffer { get; set; }
    public readonly Processor Processor;

    #endregion

    #region Constructor

    public Game(string password, int maxPlayers)
    {
        Id = Guid.NewGuid();
        Password = HashPassword(password);
        MaxPlayers = maxPlayers;
        DrawPile = new Queue<Card>();
        DiscardPile = new Stack<Card>();
        Players = new Queue<Player>(MaxPlayers);
        DrawBuffer = 0;
        Processor = new Processor(this);
    }

    #endregion

    #region Public Methods

    public void Start()
    {
        Running = true;
        Processor.Setup();
    }

    public bool CheckPassword(string password) => Verify(password, Password);

    public bool HasPlayer(Guid id) => Players.Any(player => player.Id == id);

    public Player GetPlayer(Guid id) => Players.First(p => p.Id == id);

    public void RemovePlayer(Guid id)
    {
        List<Player> list = new();
        list.AddRange(Players);
        list.Remove(list.First(p => p.Id == id));
        Players = new Queue<Player>(list);
    }

    #endregion
}