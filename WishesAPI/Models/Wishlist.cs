using Microsoft.AspNetCore.Identity;

namespace WishesAPI.Models;

public class Wishlist
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; } = "";
    public string Emoji { get; set; } = "";
    public DateTime? TargetDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublic { get; set; }
    
    public IdentityUser User { get; set; }
    public ICollection<Wish> Wishes { get; }
}