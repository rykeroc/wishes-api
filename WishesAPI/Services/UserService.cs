using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WishesAPI.Errors;
using WishesAPI.Models.DTO;

namespace WishesAPI.Services;

public interface IUserService
{
    Task<UserResult> GetUser(ClaimsPrincipal userPrincipal);
    Task<UserResult> UpdateUser(ClaimsPrincipal userPrincipal, UpdateUserDto updateData);
}

public class UserService(
    UserManager<IdentityUser> userManager,
    ILogger<UserService> logger
): IUserService
{
    public async Task<UserResult> GetUser(ClaimsPrincipal userPrincipal)
    {
        logger.LogTrace("GetUserData Initiated");
        
        logger.LogDebug("Retrieving user using user claims principal: {claims}", userPrincipal);
        var user = await userManager.GetUserAsync(userPrincipal);
        if (user != null) return UserResult.Success(user);
        logger.LogInformation("User not found. Claims principal: {claims}", userPrincipal);
        return UserResult.Failure(nameof(UserErrors.UserNotFound));
    }

    public async Task<UserResult> UpdateUser(ClaimsPrincipal userPrincipal, UpdateUserDto updateData)
    {
        logger.LogTrace("UpdateUser Initiated");

        // Get user
        logger.LogDebug("Retrieving user using user claims principal: {claims}", userPrincipal);
        var user = await userManager.GetUserAsync(userPrincipal);
        if (user == null)
        {
            logger.LogInformation("User not found. Claims principal: {claims}", userPrincipal);
            return UserResult.Failure(nameof(UserErrors.UserNotFound));
        }

        // Update username
        logger.LogDebug("Update username for user with ID {id} to {username}", user.Id, updateData.Username);
        var result = await userManager.SetUserNameAsync(user, updateData.Username);
        if (!result.Succeeded)
        {
           logger.LogInformation("Error update username for user with ID {id}: {errors}", user.Id, result.Errors);
           return UserResult.Failure(result.Errors.ElementAt(0).Code);
        } 
        logger.LogInformation("Username updated to {username} for user with ID {id}", user.UserName, user.Id);
        
        return UserResult.Success(user);
    }
}