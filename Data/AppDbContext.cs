using Microsoft.EntityFrameworkCore;
using MottuApi.Core.Entities;

namespace MottuApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para Oracle: converte nomes de tabelas e colunas para maiúsculas
            // Isso evita problemas com case-sensitivity do Oracle
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Converte nome da tabela para maiúsculas
                var tableName = entityType.GetTableName();
                if (tableName != null)
                {
                    entityType.SetTableName(tableName.ToUpper());
                }

                // Converte nomes das colunas para maiúsculas
                foreach (var property in entityType.GetProperties())
                {
                    var columnName = property.GetColumnName();
                    if (columnName != null)
                    {
                        property.SetColumnName(columnName.ToUpper());
                    }
                }
            }

            // --- Configuração para Moto ---
            modelBuilder.Entity<Moto>(entity =>
            {
                // Garante que a Placa da moto seja única no banco de dados.
                // Isso cria um "Unique Index" na tabela.
                entity.HasIndex(e => e.Placa).IsUnique();
            });

            // --- Configuração para Cliente ---
            modelBuilder.Entity<Cliente>(entity =>
            {
                // Garante que o CPF e a CNH sejam únicos.
                entity.HasIndex(e => e.Cpf).IsUnique();
                entity.HasIndex(e => e.NumeroCNH).IsUnique();
            });

            // --- Configuração para Locacao ---
            modelBuilder.Entity<Locacao>(entity =>
            {
                // Configura a precisão das colunas de valor monetário para o banco de dados.
                entity.Property(e => e.ValorDiaria).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ValorTotal).HasColumnType("decimal(18, 2)");

                // Define o relacionamento com Cliente
                entity.HasOne(l => l.Cliente)
                      .WithMany(c => c.Locacoes)
                      .HasForeignKey(l => l.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict); // Impede que um cliente seja deletado se tiver locações.

                // Define o relacionamento com Moto
                entity.HasOne(l => l.Moto)
                      .WithMany(m => m.Locacoes)
                      .HasForeignKey(l => l.MotoId)
                      .OnDelete(DeleteBehavior.Restrict); // Impede que uma moto seja deletada se tiver locações.
            });
        }
    }
}