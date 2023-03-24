using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Een.Model;

[PrimaryKey(nameof(Id))]
public class User : Player
{
    public string Password { get; set; }
    public int Wins { get; set; }
    public int Loses { get; set; }

    // For EFCore, never used
    public User()
    {
    }

    public User(string username, string password) : base(username)
    {
        Password = HashPassword(password);
        Wins = 0;
        Loses = 0;
    }

    public bool CheckPassword(string password) => Verify(password, Password);
}