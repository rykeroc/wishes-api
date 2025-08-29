namespace WishesAPI.Models;

public class Account
{
    public string Type { get; set; }
    public string Provider { get; set; }
    public string ProviderAccountId { get; set; }
    public string AccessToken { get; set; }
    public string? RefreshToken { get; set; } = null;
    public string? IdToken { get; set; } = null;
    public int? ExpiresAt { get; set; } = null;
    public string? TokenType { get; set; } = null;
    public string? Scope { get; set; } = null;
    public string? SessionState { get; set; } = null;

    public User user { get; set; }
}