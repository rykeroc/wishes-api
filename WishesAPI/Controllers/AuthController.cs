using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishesAPI.Services;

namespace WishesAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController(
    ILogger<AuthController> logger,
    IConfiguration configuration,
    IAuthService authService
) : ControllerBase
{
    [HttpGet]
    [Route("google/login")]
    public ActionResult GoogleLogin(string? returnUrl = null)
    {
        logger.LogTrace("GoogleLogin initiated with returnUrl: {ReturnUrl}", returnUrl);
        
        var redirectUri = Url.Action("GoogleCallback", new { returnUrl });
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUri,
            Items =
            {
                ["LoginProvider"] = GoogleDefaults.AuthenticationScheme
            }
        };

        logger.LogDebug("Initiating Google login with redirect URI: {RedirectUri}", properties.RedirectUri);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    private bool IsSafeRedirect(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return false;

        // Allow local URLs
        if (Url.IsLocalUrl(returnUrl))
            return true;

        // Optionally, allow configured client domains
        var allowedBaseUrl = configuration["ClientApplication:BaseUrl"];
        return allowedBaseUrl != null && returnUrl.StartsWith(allowedBaseUrl, StringComparison.OrdinalIgnoreCase);
    }
    
    [HttpGet]
    [Route("google/callback")]
    public async Task<ActionResult> GoogleCallback(string? returnUrl = null)
    {
        logger.LogTrace("GoogleCallback initiated with returnUrl: {ReturnUrl}", returnUrl);

        var loginResult = await authService.LoginWithProvider();
        if (!loginResult.Succeeded)
        {
            return Problem(loginResult.Error, statusCode:StatusCodes.Status401Unauthorized);
        }
        
        // Default fallback if returnUrl is missing or unsafe
        var defaultCallback = configuration["ClientApplication:LoginCallback"]!;
        var redirectUrl = IsSafeRedirect(returnUrl) ? returnUrl! : defaultCallback;

        // Redirect to client application
        return Redirect(redirectUrl);
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
        logger.LogTrace("AccessDenied initiated");
        
        return Forbid();
    }
}