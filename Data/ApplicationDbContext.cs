using Microsoft.EntityFrameworkCore;
using TechRepair.Models;

namespace TechRepair.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; } = default!;
    public DbSet<Equipamento> Equipamentos { get; set; } = default!;
    public DbSet<Tecnico> Tecnicos { get; set; } = default!;
    public DbSet<Reparacao> Reparacoes { get; set; } = default!;
    public DbSet<Intervencao> Intervencoes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cliente
        modelBuilder.Entity<Cliente>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Nome).IsRequired().HasMaxLength(150);
            e.Property(c => c.NIF).IsRequired().HasMaxLength(9);
            e.Property(c => c.Email).IsRequired().HasMaxLength(150);
            e.Property(c => c.Telefone).HasMaxLength(30);
            e.Property(c => c.Morada).HasMaxLength(200);
            e.Property(c => c.CodigoPostal).HasMaxLength(10);
            e.Property(c => c.Localidade).HasMaxLength(100);
            e.Property(c => c.DataRegisto).IsRequired();
        });

        // Equipamento
        modelBuilder.Entity<Equipamento>(e =>
        {
            e.HasKey(eq => eq.Id);
            e.Property(eq => eq.TipoEquipamento).IsRequired().HasMaxLength(80);
            e.Property(eq => eq.Marca).HasMaxLength(80);
            e.Property(eq => eq.Modelo).HasMaxLength(80);
            e.Property(eq => eq.NumeroSerie).HasMaxLength(80);
            e.Property(eq => eq.Descricao).HasMaxLength(500);
            e.Property(eq => eq.DataRegisto).IsRequired();

            e.HasOne(eq => eq.Cliente)
             .WithMany(c => c.Equipamentos)
             .HasForeignKey(eq => eq.ClienteId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Tecnico
        modelBuilder.Entity<Tecnico>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Nome).IsRequired().HasMaxLength(150);
            e.Property(t => t.Email).IsRequired().HasMaxLength(150);
            e.Property(t => t.Telefone).HasMaxLength(30);
            e.Property(t => t.Especialidade).HasMaxLength(100);
            e.Property(t => t.Ativo).IsRequired();
        });

        // Reparacao
        modelBuilder.Entity<Reparacao>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.DescricaoAvaria).IsRequired().HasMaxLength(1000);
            e.Property(r => r.Diagnostico).HasMaxLength(2000);
            e.Property(r => r.Estado).IsRequired().HasMaxLength(50);
            e.Property(r => r.Observacoes).HasMaxLength(1000);
            e.Property(r => r.DataEntrada).IsRequired();

            e.HasOne(r => r.Equipamento)
             .WithMany(eq => eq.Reparacoes)
             .HasForeignKey(r => r.EquipamentoId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Tecnico)
             .WithMany(t => t.Reparacoes)
             .HasForeignKey(r => r.TecnicoId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Intervencao
        modelBuilder.Entity<Intervencao>(e =>
        {
            e.HasKey(i => i.Id);
            e.Property(i => i.DescricaoTrabalho).IsRequired().HasMaxLength(2000);
            e.Property(i => i.PecasUtilizadas).HasMaxLength(1000);
            e.Property(i => i.TempoTrabalho).HasPrecision(10, 2);
            e.Property(i => i.CustoMaoObra).HasPrecision(18, 2);
            e.Property(i => i.CustoPecas).HasPrecision(18, 2);
            e.Property(i => i.Observacoes).HasMaxLength(1000);
            e.Property(i => i.DataIntervencao).IsRequired();

            e.HasOne(i => i.Reparacao)
             .WithMany(r => r.Intervencoes)
             .HasForeignKey(i => i.ReparacaoId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
