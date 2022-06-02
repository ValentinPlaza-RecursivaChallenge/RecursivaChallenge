using Microsoft.EntityFrameworkCore;
using RecursivaChallenge.Models.Entity;

namespace RecursivaChallenge.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
        public DbSet<Socio> Socios { get; set; }
    }
}
