using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FootballManager.Controllers;
/// <summary>
/// User Controller
/// </summary>
[ApiController]
[Route("[controller]"), Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration configuration;
    /// <summary>
    /// User controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="configuration"></param>
    public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.configuration = configuration;
    }
    /// <summary>
    /// Get profile
    /// </summary>
    /// <returns>A profile</returns>
    [HttpGet("profile")]
    public async Task<ActionResult> Profile()
    {
        var identity = User.Identity;
        if (identity is not ClaimsIdentity)
        {
            return BadRequest("ERR_IDENTITY_EMPTY");
        }
        var claims = (ClaimsIdentity)identity;
        return Ok(new
        {
            UserId = int.Parse(claims.FindFirst("UserId")?.Value ?? "0"),
            Email = claims.FindFirst("Email")?.Value ?? string.Empty,
            Username = claims.FindFirst("Username")?.Value ?? string.Empty,
            Role = claims.FindFirst(ClaimTypes.Role)?.Value ?? "User",
        });
    }
    /// <summary>
    /// Authenticate
    /// </summary>
    /// <param name="request">UserAuthenticate Request model</param>
    /// <returns>string token</returns>
    [HttpPost("authenticate"), AllowAnonymous]
    public async Task<ActionResult> Authenticate([FromBody] AuthenticateUserRequest request)
    {
        var user = await unitOfWork.UserRepository.GetAsync(request.Email);
        if (user is null)
        {
            return BadRequest("ERR_USER_NOT_EXIST");
        }
        var isAuthorize = await unitOfWork.UserRepository.ValidatePasswordAsync(user, request.Password);
        if (!isAuthorize)
        {
            return Unauthorized("ERR_UNAUTHORIZED");
        }
        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub,configuration["Jwt:Subject"]??string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
            new Claim("UserId",$"{user.Id}"),
            new Claim("Name",$"{user.Name}"),
            new Claim("Email",$"{user.Email}"),
            new Claim("Username",$"{user.Username}"),
            new Claim(ClaimTypes.Role, user.IsAdmin?"Admin":"User"),
        };

        // generate token that is valid for 1 hour
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? string.Empty);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Audience = configuration["Jwt:Audience"],
            Issuer = configuration["Jwt:Issuer"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var result = new
        {
            AccessToken = tokenHandler.WriteToken(token),
            TokenType = "Bearer",
            ExpiredIn = TimeSpan.FromDays(7).TotalSeconds,
        };
        return Ok(result);
    }
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List user</returns>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var users = await unitOfWork.UserRepository.GetAsync();
        return Ok(users);
    }
    /// <summary>
    /// Add a user
    /// </summary>
    /// <param name="request">Create User Request model</param>
    /// <returns>User info</returns>
    [HttpPost]
    public async Task<ActionResult> Add([FromBody] CreateUserRequest request)
    {
        var result = await unitOfWork.UserRepository.AddAsync(new User
        {
            Email = request.Email,
            IsAdmin = request.IsAdmin,
            MemberId = request.MemberId,
            Username = request.Username,
            Name = request.Name,
        }, request.Password);
        return Ok(result);
    }
    /// <summary>
    /// Add a user
    /// </summary>
    /// <param name="request">User Authenticate request model</param>
    /// <returns>User info</returns>
    [HttpPost("init"), AllowAnonymous]
    public async Task<ActionResult> InitialAdmin()
    {
        var result = await unitOfWork.UserRepository.AddAsync(new()
        {
            Name = "AnLe",
            Email = "an.lethanh3@gmail.com",
            IsAdmin = true,
            MemberId = 0,
            Username = "anle",
        }, "12345678");
        return Ok(result);
    }
    /// <summary>
    /// Update a user information
    /// </summary>
    /// <param name="user">user model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPut]
    public async Task<ActionResult> Update(User user)
    {
        var result = await unitOfWork.UserRepository.UpdateAsync(user);
        return Ok(result);
    }
    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">user id</param>
    /// <returns>true: deleted, false: error</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await unitOfWork.UserRepository.DeleteAsync(id);
        return Ok(result);
    }
}
