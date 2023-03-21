using Een.Model;
using Newtonsoft.Json;

namespace Een.Api.Responses;

public class LoginResponse
{
    public string Token { get; }
    public User User { get; }
    
    public LoginResponse(string token, User user)
    {
        Token = token;
        User = user;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}