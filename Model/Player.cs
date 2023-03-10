namespace Model;

public class Player
{
    #region Properties

    public Guid Id { get; }
    public string Username { get; }
    public string ProfileImage { get; }

    public List<Card> Cards { get; set; }

    #endregion

    #region Constructor
    
    public Player(string username)
    {
        Username = username;
        Id = Guid.NewGuid();
        Cards = new List<Card>();
        ProfileImage = $"https://api.dicebear.com/5.x/identicon/svg?backgroundColor=ffffff&seed={Id}";
    }

    #endregion

    #region Pubic Methods

    /// <summary>
    /// Adds a <see cref="Card"/> to the <see cref="Player"/>
    /// </summary>
    /// <param name="card"><see cref="Card"/> to add.</param>
    public void AddCard(Card card) => Cards.Add(card);

    /// <summary>
    /// Check if a <see cref="Player"/> has a <see cref="Card"/>
    /// </summary>
    /// <param name="card"><see cref="Card"/> to remove.</param>
    public bool HasCard(Card card)
    {
        if (card.Value == CardValue.Wild)
        {
            return Cards.Any(c => c.Value == card.Value);
        }

        return Cards.Any(c => c.Value == card.Value && c.Color == card.Color);
    }

    /// <summary>
    /// Removes a <see cref="Card"/> from the <see cref="Player"/>
    /// </summary>
    /// <param name="card"><see cref="Card"/> to remove.</param>
    public void RemoveCard(Card card)
    {
        Card c = Cards.First(c => c.Color == card.Color && c.Value == card.Value);

        if (card.Value == CardValue.Wild)
        {
            c = Cards.First(c => c.Value == card.Value);
        }

        Cards.Remove(c);
    }

    #endregion
}