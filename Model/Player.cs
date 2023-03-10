using System.Drawing;

namespace Model;

public class Player
{
    public Guid Id { get; }
    public string Username { get; }
    public string ProfileImage { get; }

    public List<Card> Cards { get; set; }

    public void RemoveCard(Card card)
    {
        Card c = Cards.First(c => c.Color == card.Color && c.Value == card.Value);
        Cards.Remove(c);
    }

    public void RemoveCard(CardValue value)
    {
        Card c = Cards.First(c => c.Value == value);
        Cards.Remove(c);
    }

    public Player(string username)
    {
        Username = username;
        Id = Guid.NewGuid();
        Cards = new List<Card>();
        ProfileImage = $"https://api.dicebear.com/5.x/identicon/svg?backgroundColor=ffffff&seed={Id}";
    }
}