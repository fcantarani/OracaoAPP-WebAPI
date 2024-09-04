using Microsoft.EntityFrameworkCore;
using OracaoApp.Data.DbModels;

namespace OracaoApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Prayer> Prayers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Testimony> Testimonies { get; set; }

}
