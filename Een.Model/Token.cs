namespace Een.Model;

public class Token
{
    public User User { get; }
    
    public Token(User user)
    {
        User = user;
    }
}