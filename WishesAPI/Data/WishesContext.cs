using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WishesAPI.Models;

namespace WishesAPI.Data;

public class WishesContext: IdentityDbContext
{
    public DbSet<Wish> Wishes { get; set; } = null!;
    public DbSet<Wishlist> Wishlists { get; set; } = null!;
        
    public WishesContext(DbContextOptions options) : base(options)
    {
    }
}