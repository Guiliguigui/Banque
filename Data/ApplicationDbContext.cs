using Banque.Models;
using Microsoft.EntityFrameworkCore;

namespace Banque.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<CompteEpargne> ComptesEpargnes { get; set; }
        public DbSet<ComptePayant> ComptesPayants { get; set; }
        public DbSet<Operation> Operations { get; set; }

        //OnCreation n'est pas necessaire ici car j'ai mis un equivalent dans Startup.cs (lignes 30-31)
    }
}
