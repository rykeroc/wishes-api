namespace WishesAPI.Models;

public class Wishlist
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; } = "";
    public string Emoji { get; set; } = "";
    public DateTime? TargetDate { get; set; } = null;
    public DateTime? CreatedAt { get; set; } = null;
    public DateTime? UpdatedAt { get; set; } = null;
    public bool IsPublic { get; set; } = false;
    
    public User User { get; set; }
    public ICollection<Wish> Wishes { get; }
}