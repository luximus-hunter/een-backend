using Een.Data;
using Een.Model;
using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;

namespace Een.Api.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? t = context.Request.Headers["token"];
        bool authed = false;

        if (t != null)
        {
            try
            {
                string? json = JwtBuilder.Create()
                    .WithAlgorithm(new NoneAlgorithm())
                    .Decode(t);

                Token? token = JsonConvert.DeserializeObject<Token>(json);

                Database db = new();
                
                if (token == null) return;
                if (!db.Users.Any(u => u.Username == token.User.Username && u.Password == token.User.Password)) return;

                authed = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        context.Request.Headers["Authed"] = authed.ToString();

        await _next(context);
    }
}

public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}