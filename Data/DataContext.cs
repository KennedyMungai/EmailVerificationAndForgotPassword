using Microsoft.EntityFrameworkCore;

using EmailVerificationAndForgotPassword.Models;

namespace EmailVerificationAndForgotPassword.Data;

public class DataContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    
    

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
}