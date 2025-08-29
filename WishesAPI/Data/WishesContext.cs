using Microsoft.EntityFrameworkCore;
using WishesAPI.Models;

namespace WishesAPI.Data;

public class WishesContext: DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Wish> Wishes { get; set; } = null!;
    public DbSet<Wishlist> Wishlists { get; set; } = null!;
        
    public WishesContext(DbContextOptions options) : base(options)
    {
    }
}