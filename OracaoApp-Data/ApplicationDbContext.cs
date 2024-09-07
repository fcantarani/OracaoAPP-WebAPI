using Microsoft.EntityFrameworkCore;
using OracaoApp.Data.DbModels;

namespace OracaoApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Prayer> Prayers { get; set; }
    public DbSet<PrayerCategory> PrayerCategories { get; set; }
    public DbSet<PrayerComment> PrayerComments { get; set; }
    public DbSet<Testimony> Testimonies { get; set; }
    public DbSet<TestimonyComment> TestimonyComments { get; set; }

}
