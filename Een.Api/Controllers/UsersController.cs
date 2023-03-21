using System.Globalization;
using System.Net;
using Een.Data;
using Een.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Een.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("users/{id:guid}")]
public class UsersController : ControllerBase
{
    #region Fields

    private readonly ILogger<UsersController> _logger;

    #endregion

    #region Constructor

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
        _logger.Log(LogLevel.Information, @"Users route created.");
    }

    #endregion

    #region Private Methods

    private static string ErrorMessage(string message) => $"{{\"status\": false, \"message\": \"{message}\"}}";

    private static bool Authenticated(HttpRequest r) =>
        r.Headers["Authed"] != StringValues.Empty && bool.Parse(r.Headers["Authed"].ToString());

    private static bool IsUrl(string url)
    {
        Uri? uriResult;
        return Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    #endregion

    #region Public Methods

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(Guid id)
    {
        if (!Authenticated(Request))
        {
            return Unauthorized(ErrorMessage("You need to be logged in to use this route."));
        }

        Database db = new();

        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found."));
        }

        User user = db.Users.First(u => u.Id == id);

        return Ok(user);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutProfileImage(Guid id, string profileImage)
    {
        if (!Authenticated(Request))
        {
            return Unauthorized(ErrorMessage("You need to be logged in to use this route."));
        }

        Database db = new();

        if (!IsUrl(profileImage))
        {
            return BadRequest(ErrorMessage("URL is not an image."));
        }

        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found."));
        }

        User dbUser = db.Users.First(u => u.Id == id);
        dbUser.ProfileImage = profileImage;
        db.SaveChangesAsync();

        return Ok(dbUser);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        if (!Authenticated(Request))
        {
            return Unauthorized(ErrorMessage("You need to be logged in to use this route."));
        }

        Database db = new();

        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found."));
        }

        User user = db.Users.First(u => u.Id == id);

        db.Users.Remove(user);

        return Ok();
    }

    #endregion
}