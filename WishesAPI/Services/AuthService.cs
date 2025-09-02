using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace WishesAPI.Services;

public interface IAuthService
{
    string? FindEmail(IEnumerable<Claim> claims);
    string? FindProviderKey(IEnumerable<Claim> claims);
    Task<IdentityUser> FindProviderUserWithEmailAsync(string email, string provider, string providerKey);
    Task SignInUserAsync(IdentityUser user);
}

public class AuthService(
    ILogger<AuthService> logger,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager
): IAuthService
{
    public string? FindEmail(IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    }

    public string? FindProviderKey(IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<IdentityUser> FindProviderUserWithEmailAsync(string email, string provider, string providerKey)
    {
        // Find user with email
        var user = await userManager.FindByEmailAsync(email);
        // User exists
        if (user != null) return user;
        
        // Create new user
        user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        var loginInfo = new UserLoginInfo(provider, providerKey, providerKey);

        // Persist new user and provider login information
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            logger.LogError("Error creating user with email {email}: {errors}", email, result.Errors);
            throw new AuthenticationFailureException(result.Errors.ToString());
        }
        
        result = await userManager.AddLoginAsync(user, loginInfo);
        if (!result.Succeeded)
        {
            logger.LogError("Error adding login info for user with ID {userId}: {errors}", user.Id, result.Errors);
            throw new AuthenticationFailureException(result.Errors.ToString());
        }

        return user;
    }

    public async Task SignInUserAsync(IdentityUser user)
    {
        await signInManager.SignInAsync(user, isPersistent: false);
    }
}