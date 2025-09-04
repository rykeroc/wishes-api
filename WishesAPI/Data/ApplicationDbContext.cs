using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WishesAPI.Models;

namespace WishesAPI.Data;

public class ApplicationDbContext: IdentityDbContext
{
    public DbSet<Wish> Wishes { get; set; } = null!;
    public DbSet<Wishlist> Wishlists { get; set; } = null!;
        
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}