using Microsoft.EntityFrameworkCore;

namespace CustomerRegister.Infra;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
