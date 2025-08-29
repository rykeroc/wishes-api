namespace WishesAPI.Models;

public class Wish
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int? PriceAmount { get; set; } = null;
    public string? PriceCurrency { get; set; } = null;
    public string? ProductUrl { get; set; } = null;
    public int Priority { get; set; } = 0;
    public DateTime? CreatedAt { get; set; } = null;
    public DateTime? UpdatedAt { get; set; } = null;
    
    public Wishlist Wishlist { get; set; }
}