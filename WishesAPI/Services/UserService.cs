using Microsoft.AspNetCore.Identity;

namespace WishesAPI.Services;

public interface IUserService
{
    Task<IdentityUser?> GetUser();
    // Task<IdentityUser?> UpdateUser();
}

public class UserService(
    UserManager<IdentityUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<UserService> logger
): IUserService
{
    public async Task<IdentityUser?> GetUser()
    {
        logger.LogTrace("GetUserData Initiated");
        
        logger.LogDebug("Retrieving current user principal from HTTP context.");
        var userPrincipal = httpContextAccessor.HttpContext?.User;
        if (userPrincipal == null)
        {
            logger.LogInformation("No user claims principal in current HTTP context.");
            return null;
        }

        logger.LogDebug("Retrieving user using user claims principal: {claims}", userPrincipal);
        var user = await userManager.GetUserAsync(userPrincipal);
        if (user != null) return user;
        logger.LogInformation("User not found. Claims: {claims}", userPrincipal);
        return null;

    }

    // public Task<IdentityUser?> UpdateUser()
    // {
    //     throw new NotImplementedException();
    // }
}