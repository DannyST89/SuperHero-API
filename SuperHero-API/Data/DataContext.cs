using Microsoft.EntityFrameworkCore;
using SuperHero_API.Models;

namespace SuperHero_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }


        // Si quieres ver una entidad en la base de datos como una tabla
        public DbSet<SuperHero> SuperHeroes => Set<SuperHero>();
    }
}
