using System.Text.RegularExpressions;

namespace WishesAPI.Helpers;

public partial class UserHelper
{
    [GeneratedRegex("[.!#$%&'*+-/=?^_{|}~]")]
    private static partial Regex ValidEmailSpecialCharacters();
    
    public static string GetDefaultUsernameFromEmail(string email)
    {
        return ValidEmailSpecialCharacters().Replace(email.Split("@")[0], "-");
    }
}