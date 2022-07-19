using Microsoft.EntityFrameworkCore;

using EmailVerificationAndForgotPassword.Models;

namespace EmailVerificationAndForgotPassword.Data;

public class DataContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=SomeRandomName; Trusted_Connection=true; MultipleActiveResultSets=true;");
    }
}