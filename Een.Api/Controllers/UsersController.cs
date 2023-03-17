using Een.Data;
using Een.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Een.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    #region Fields

    private readonly ILogger<UsersController> _logger;

    #endregion

    #region Constructor

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
        _logger.Log(LogLevel.Information, @"Users route created");
    }

    #endregion

    #region Private Methods

    private static string ErrorMessage(string message) => $"{{\"status\": false, \"message\": \"{message}\"}}";

    #endregion

    #region Public Methods

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    public IActionResult Get(Guid id)
    {
        Database db = new();

        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found"));
        }

        User user = db.Users.First(u => u.Id == id);

        return Ok(user);
    }

    [HttpPut("{id:guid}/profile-image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutProfileImage(Guid id, string profileImage)
    {
        Database db = new();

        if (false) //TODO: Check for integrity
        {
            return BadRequest(ErrorMessage("Profile image was invalid."));
        }

        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found"));
        }

        User dbUser = db.Users.First(u => u.Id == id);
        dbUser.ProfileImage = profileImage;

        return Ok(dbUser);
    }

    [HttpPut("{id:guid}/wins")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutWins(Guid id, int wins)
    {
        Database db = new();

        if (false) //TODO: Check for integrity
        {
            return BadRequest(ErrorMessage("Profile image was invalid."));
        }

        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found"));
        }

        User dbUser = db.Users.First(u => u.Id == id);
        dbUser.Wins = wins;

        return Ok(dbUser);
    }

    [HttpPut("{id:guid}/loses")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutLoses(Guid id, int loses)
    {
        Database db = new();

        if (false) //TODO: Check for integrity
        {
            return BadRequest(ErrorMessage("Profile image was invalid."));
        }

        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found"));
        }

        User dbUser = db.Users.First(u => u.Id == id);
        dbUser.Loses = loses;

        return Ok(dbUser);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        Database db = new();
        
        if (!db.Users.Any(u => u.Id == id))
        {
            return NotFound(ErrorMessage("Specified user was not found"));
        }

        User user = db.Users.First(u => u.Id == id);

        db.Users.Remove(user);

        return Ok();
    }

    #endregion
}