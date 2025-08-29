using Microsoft.AspNetCore.Mvc;

namespace WishesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(ILogger<AuthController> logger) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;

    // TODO
    [HttpGet]
    [Route("/login/google")]
    public ActionResult LoginGoogle()
    {
        return new OkResult();
    }
    
    // TODO
    [HttpGet]
    [Route("/login/callback")]
    public ActionResult LoginCallback()
    {
        return new OkResult();
    }
    
    // TODO
    [HttpGet]
    [Route("/refresh")]
    public ActionResult RefreshToken()
    {
        return new OkResult();
    }
    
    // TODO
    [HttpGet]
    [Route("/logout")]
    public ActionResult Logout()
    {
        return new OkResult();
    }
}