using AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Helpers
{
    public class AuthDBContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public AuthDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ApiDatabase")).UseSnakeCaseNamingConvention();
        }
        public DbSet<User> user { get; set; }
    }
}
