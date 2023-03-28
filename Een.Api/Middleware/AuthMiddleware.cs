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
                // TODO: Validation    
                string? json = JwtBuilder.Create()
                    .WithAlgorithm(new NoneAlgorithm())
                    .Decode(t);

                Token? token = JsonConvert.DeserializeObject<Token>(json);

                if (token == null) return;
                if (Users.Get(token.User.Username, token.User.Password) == null) return;

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