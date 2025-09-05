using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishesAPI.Services;

namespace WishesAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    ILogger<AuthController> logger,
    IConfiguration configuration,
    IAuthService authService
) : ControllerBase
{
    [HttpGet]
    [Route("google/login")]
    public ActionResult GoogleLogin()
    {
        logger.LogTrace("GoogleLogin initiated");
        
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleCallback")
        };

        properties.Items["LoginProvider"] = GoogleDefaults.AuthenticationScheme;
        
        logger.LogDebug("Initiating Google login with redirect URI: {RedirectUri}", properties.RedirectUri);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet]
    [Route("google/callback")]
    public async Task<ActionResult> GoogleCallback()
    {
        logger.LogTrace("GoogleCallback initiated");

        var loginResult = await authService.LoginWithProvider();
        if (!loginResult.Succeeded)
        {
            return Problem(loginResult.Error, statusCode:StatusCodes.Status401Unauthorized);
        }
        
        var loginCallbackPath = configuration["ClientApplication:LoginCallback"]!;
        
        // Redirect to client application
        return Redirect(loginCallbackPath);
    }
    
    [HttpGet]
    [Route("logout")]
    public async Task<ActionResult> Logout()
    {
        await authService.SignOutUserAsync();
        
        var logoutCallbackPath = configuration["ClientApplication:LogoutCallback"]!;
        
        // Redirect to client application
        return Redirect(logoutCallbackPath);
    }
    
    [HttpGet]
    [Route("access-denied")]
    public IActionResult AccessDenied()
    {
        return Forbid();
    }
}