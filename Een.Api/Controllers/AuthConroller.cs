using System.Security.Cryptography.X509Certificates;
using Een.Api.Responses;
using Een.Data;
using Een.Model;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BCrypt.Net.BCrypt;

namespace Een.Api.Controllers;

[ApiController]
[Route("")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    #region Fields

    private readonly ILogger<AuthController> _logger;

    #endregion

    #region Constructor

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
        _logger.Log(LogLevel.Information, @"Auth route created");
    }

    #endregion

    #region Private Methods

    private static string ErrorMessage(string message) => $"{{\"status\": false, \"message\": \"{message}\"}}";

    #endregion

    #region Public Methods

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Login(string username, string password)
    {
        Database db = new();

        if (!db.Users.Any(u => u.Username == username))
        {
            return NotFound(ErrorMessage("Username and password combination not found."));
        }

        User user = db.Users.First(u => u.Username == username);

        if (!Verify(password, user.Password))
        {
            return NotFound(ErrorMessage("Username and password combination not found."));
        }

        string? token = JwtBuilder.Create()
            .WithAlgorithm(new NoneAlgorithm())
            .AddClaim("user", user)
            .Encode();

        return Ok(new LoginResponse(token, user));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Register(string username, string password)
    {
        Database db = new();

        if (false) //TODO: Check for integrity
        {
            return BadRequest(ErrorMessage("Username or password was invalid."));
        }

        if (db.Users.Any(u => u.Username == username))
        {
            return Conflict(ErrorMessage("Username is taken."));
        }

        User user = new(username, password);

        db.Users.Add(user);
        db.SaveChanges();

        return Ok();
    }

    #endregion
}