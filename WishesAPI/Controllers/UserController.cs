using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WishesAPI.Models.DTO.Responses;
using WishesAPI.Services;

namespace WishesAPI.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/user")]
public class UserController(
    ILogger<AuthController> logger,
    IUserService userService
): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserData()
    {
        logger.LogTrace("GetUserInfo Initiated");
        
        var user = await userService.GetUser();
        if (user == null) return Problem("An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);

        var userDataResponse = new UserData(user.Id, user.UserName, user.Email);
        return Ok(userDataResponse);
    }
}