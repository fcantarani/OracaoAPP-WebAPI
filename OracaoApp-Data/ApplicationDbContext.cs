using Microsoft.EntityFrameworkCore;
using OracaoApp_Data.DbModels;

namespace OracaoApp_Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }


    }
}
