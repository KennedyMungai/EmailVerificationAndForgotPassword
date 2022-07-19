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
}