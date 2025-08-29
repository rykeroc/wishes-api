namespace WishesAPI.Models;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
    
    public DateTime? EmailVerified { get; set; } = null;
    public string ImageUrl { get; set; } = "";
    public bool CompletedOnboarding { get; set; } = false;
    
    public ICollection<Account> Accounts { get; }
}