using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace EmailVerificationAndForgotPassword.Controllers;

[Route("api/{controller}")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataContext context;

    public UserController(DataContext context)
    {
        this.context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        if(context.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest("User already exists");
        }

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            VerificationToken = CreateRandomToken()
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok("User successfully created");
    }

    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}