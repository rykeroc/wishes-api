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
    IJwtService jwtService,
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
        IdentityUser user;
        try
        {
            user = await authService.FindProviderUserWithEmailAsync(email, provider, providerKey);
        }
        catch (AuthenticationFailureException)
        {
            return Problem(detail: "Error finding user", statusCode: StatusCodes.Status500InternalServerError);
        }
        
        // Sign in user
        await authService.SignInUserAsync(user);
        
        // Generate JWT token
        var token = jwtService.GenerateToken(user.Id, user.Email!, user.UserName!);
        logger.LogDebug("Generated token: {token}", token);

        var loginCallbackPath = configuration["ClientApplication:LoginCallback"];
        var fullLoginCallbackUrl = $"{loginCallbackPath}?token={token}";
        
        // Redirect to client application
        return Redirect(fullLoginCallbackUrl);
    }
    
    // // TODO
    // [HttpGet]
    // [Route("/refresh")]
    // public ActionResult RefreshToken()
    // {
    //     return new OkResult();
    // }
    //
    // // TODO
    // [HttpGet]
    // [Route("/logout")]
    // public ActionResult Logout()
    // {
    //     return new OkResult();
    // }
}