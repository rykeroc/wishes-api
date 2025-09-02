using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using WishesAPI.Services;

namespace WishesAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ILogger<AuthController> logger, IJwtService jwtService, IConfiguration configuration) : ControllerBase
{
    // TODO
    [HttpGet]
    [Route("google/login")]
    public ActionResult GoogleLogin()
    {
        logger.LogDebug("GoogleLogin initiated");
        
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleCallback")
        };
        
        logger.LogDebug("Initiating Google login with redirect URI: {RedirectUri}", properties.RedirectUri);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    // TODO
    [HttpGet]
    [Route("google/callback")]
    public async Task<ActionResult> GoogleCallback()
    {
        logger.LogDebug("GoogleCallback initiated");
        
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        if (!result.Succeeded)
        {
            logger.LogInformation("Google Callback authentication failed: {ex}", result.Failure);
            return BadRequest("Google authentication failed");
        }

        // Extract user information
        var claims = result.Principal.Claims;
        var claimsList = claims.ToList();
        var userId = claimsList.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claimsList.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claimsList.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        logger.LogDebug("Token claims: {claims}", claims);
        
        // Generate JWT token
        var token = jwtService.GenerateToken(userId!, email!, name!);
        
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