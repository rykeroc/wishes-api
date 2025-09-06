using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WishesAPI.Errors;
using WishesAPI.Helpers;

namespace WishesAPI.Services;

public interface IAuthService
{
    Task<AuthResult> LoginWithProvider();
    Task SignOutUserAsync();
}

public class AuthService(
    ILogger<AuthService> logger,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager
): IAuthService
{
    public async Task<AuthResult> LoginWithProvider()
    {
        // Get external login information from provider sign in
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null)
        {
            // No login information found for the current user.
            // This means they are not signed in via the third party
            return AuthResult.Failure(nameof(AuthErrors.InvalidExternalLoginInfo));
        }
        
        // Attempt to sign in with third party login information 
        var loginResult = await signInManager.ExternalLoginSignInAsync(
            externalLoginInfo.LoginProvider, 
            externalLoginInfo.ProviderKey, 
            isPersistent: true, 
            bypassTwoFactor: true
        ); 
        if (loginResult.Succeeded) return AuthResult.Success();
        
        IdentityError error;
        // Create new user
        var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        var username = UserHelper.GetDefaultUsernameFromEmail(email!);
        var user = new IdentityUser
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true
        };
        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            error = createResult.Errors.ToList()[0];
            logger.LogError("Error creating user with email {email}: {error}", email, error);
            return AuthResult.Failure(error.Code);
        }
        
        // Create external login information for the new user
        var loginInfo = new UserLoginInfo(
            externalLoginInfo.LoginProvider, 
            externalLoginInfo.ProviderKey,  
            externalLoginInfo.LoginProvider
        );
        createResult = await userManager.AddLoginAsync(user, loginInfo);
        if (!createResult.Succeeded)
        {
            error = createResult.Errors.ToList()[0];
            logger.LogError("Error adding login info for user with ID {userId}: {error}", user.Id, error);
            return AuthResult.Failure(error.Code);
        }
        
        // Sign in the new user
        // Should always succeed unless there are infrastructure issues
        await signInManager.ExternalLoginSignInAsync(
            externalLoginInfo.LoginProvider, 
            externalLoginInfo.ProviderKey, 
            isPersistent: true, 
            bypassTwoFactor: true
        );
        
        // Return success
        return AuthResult.Success();
    }
    
    public Task SignOutUserAsync()
    {
        return signInManager.SignOutAsync();
    }
}