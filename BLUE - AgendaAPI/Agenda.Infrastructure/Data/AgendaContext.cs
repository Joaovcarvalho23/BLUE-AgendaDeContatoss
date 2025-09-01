using Agenda.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Data
{
    public class AgendaContext : DbContext
    {
        public AgendaContext(DbContextOptions<AgendaContext> options) : base(options) { }
        
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contato>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Usuario>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.Id).ValueGeneratedOnAdd();

                e.Property(u => u.Nome).IsRequired().HasMaxLength(100);
                e.Property(u => u.Email).IsRequired().HasMaxLength(150);
                e.Property(u => u.Telefone).HasMaxLength(20);
                e.Property(u => u.CPF).IsRequired().HasMaxLength(14);
                e.Property(u => u.SenhaHash).IsRequired();

                // únicos
                e.HasIndex(u => u.Email).IsUnique();
                e.HasIndex(u => u.CPF).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}