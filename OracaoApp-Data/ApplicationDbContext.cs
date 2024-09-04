using Microsoft.EntityFrameworkCore;
using OracaoApp.Data.DbModels;

namespace OracaoApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }


    }
}
