namespace WishesAPI.Models;

public class Wish
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int? PriceAmount { get; set; }
    public string? PriceCurrency { get; set; }
    public string? ProductUrl { get; set; }
    public int Priority { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Wishlist Wishlist { get; set; }
}