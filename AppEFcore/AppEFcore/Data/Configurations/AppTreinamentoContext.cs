using AppEFcore.Domain;
using Microsoft.EntityFrameworkCore;

namespace AppEFcore.Data.Configurations
{
    public class AppTreinamentoContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=10.10.1.15;Database=DBEFcore;Trusted_Connection=True;");
               
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // modelBuilder.ApplyConfiguration(new ClienteConfiguration());

            //Vai procurar no meu assemble pelo Fluent Api.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppTreinamentoContext).Assembly);
        }

    }
}
