using System.Text.RegularExpressions;

namespace WishesAPI.Helpers;

public partial class UserHelper
{
    [GeneratedRegex("[.!#$%&'*+-/=?^_{|}~]")]
    private static partial Regex InvalidSpecialCharacters();

    public const string AllowedUsernameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
    
    public static string GetDefaultUsernameFromEmail(string email)
    {
        return InvalidSpecialCharacters().Replace(email.Split("@")[0], "-");
    }
}