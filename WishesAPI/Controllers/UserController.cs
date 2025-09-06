using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WishesAPI.Models.DTO;
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
        var userPrincipal = HttpContext.User;
        var userResult = await userService.GetUser(userPrincipal);
        if (!userResult.Succeeded)
        {
            return Problem(userResult.Error,
                statusCode: StatusCodes.Status500InternalServerError);
        }

        var user = userResult.IdentityUser!;
        var userDto = new UserDto(user.Id, user.UserName, user.Email);
        return Ok(userDto);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateBasicUserData([FromBody] UpdateUserDto body)
    {
        logger.LogTrace("UpdateBasicUserData Initiated");
        var userPrincipal = HttpContext.User;
        var updateResult = await userService.UpdateUser(userPrincipal, body);
        if (!updateResult.Succeeded) return BadRequest(updateResult.Error);
        
        var user = updateResult.IdentityUser!;
        var userDto = new UserDto(user.Id, user.UserName, user.Email);
        return Ok(userDto);
    }
}