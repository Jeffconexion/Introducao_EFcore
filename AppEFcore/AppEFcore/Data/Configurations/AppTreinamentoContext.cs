using AppEFcore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AppEFcore.Data.Configurations
{
    public class AppTreinamentoContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        //Para analisar resultados de consulta e outros procedimentos.
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=DESKTOP-EG5GF5V\\SQLEXPRESS;Database=DBEFcore;Trusted_Connection=True;");
            //EnableRetryOnFailure: configuração para falhas ao tentar conectar ao banco de dados.
            
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=DESKTOP-EG5GF5V\\SQLEXPRESS;Database=DBEFcore;Trusted_Connection=True;",
                 p => p.EnableRetryOnFailure(
                     maxRetryCount: 2,
                     maxRetryDelay: TimeSpan.FromSeconds(5),
                     errorNumbersToAdd: null).MigrationsHistoryTable("curso_ef_core"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Forma tradicional: modelBuilder.ApplyConfiguration(new ClienteConfiguration());

            //Vai procurar no meu assemble pelo Fluent Api.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppTreinamentoContext).Assembly);

            //Atribuir varchar nas propriedades.
            //MapearPropriedadesEsquecidas(modelBuilder);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType())
                        && !property.GetMaxLength().HasValue)
                    {
                        //property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}
