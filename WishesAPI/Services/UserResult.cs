using Microsoft.AspNetCore.Identity;

namespace WishesAPI.Services;

public class UserResult
{
    public bool Succeeded { get; set; }
    public IdentityUser? IdentityUser { get; set; }
    public string? Error { get; set; }

    public static UserResult Success(IdentityUser identityUser) =>
        new UserResult
        {
            Succeeded = true,
            IdentityUser = identityUser
        };
    public static UserResult Failure(string? error) =>
        new UserResult
        {
            Succeeded = false,
            IdentityUser = null,
            Error = error
        };
}