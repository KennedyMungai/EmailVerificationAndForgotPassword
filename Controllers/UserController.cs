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

    #region Http Methods
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if(user == null)
        {
            return BadRequest("User Not Found");
        }

        if(user.VerifiedAt == null)
        {
            return BadRequest("Not verified");
        }

        if (!VerifiedPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong password");
        }
    }

    #endregion

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

    private bool VerifyPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}