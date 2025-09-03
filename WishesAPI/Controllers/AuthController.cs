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
        
        logger.LogDebug("Initiating Google login with redirect URI: {RedirectUri}", properties.RedirectUri);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet]
    [Route("google/callback")]
    public async Task<ActionResult> GoogleCallback()
    {
        logger.LogTrace("GoogleCallback initiated");
        
        // Get authentication details
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (!result.Succeeded)
        {
            logger.LogInformation("Google Callback authentication failed: {ex}", result.Failure);
            return BadRequest("Error loading external login information from Google");
        }

        // Extract user information
        var claims = result.Principal.Claims.ToList();
        logger.LogDebug("Token claims: {claims}", claims);

        var email = authService.FindEmail(claims);
        if (email == null)
        {
            logger.LogInformation("Email from claims is missing: {claims}", claims);
            return BadRequest("Error loading external login information from Google");
        }
        var providerKey = authService.FindProviderKey(claims);
        if (providerKey == null)
        {
            logger.LogInformation("Provider key from claims is missing: {claims}", claims);
            return BadRequest("Error loading external login information from Google");
        }
        
        // Get user with email
        const string provider = "Google";
        
        // Adds auth cookie to response for user
        await authService.ExternalSignInUserAsync(provider, providerKey, isPersistent: true, bypassTwoFactor: true);

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
}