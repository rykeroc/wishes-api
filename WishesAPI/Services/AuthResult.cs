namespace WishesAPI.Services;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }

    public static AuthResult Success() => new AuthResult { Succeeded = true };
    public static AuthResult Failure(string error) => new AuthResult { Succeeded = false, Error = error };
}