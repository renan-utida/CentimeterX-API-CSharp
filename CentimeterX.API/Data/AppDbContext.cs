using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EstacaoBase> EstacoesBase { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rover> Rovers { get; set; }
        public DbSet<MaquinaAgricola> MaquinasAgricolas { get; set; }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<VeiculoAutonomo> VeiculosAutonomos { get; set; }
        public DbSet<SessaoCorrecao> SessoesCorrecao { get; set; }
        public DbSet<Ocorrencia> Ocorrencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Herança Rover → discriminator TIPO_ROVER
            modelBuilder.Entity<Rover>()
                .HasDiscriminator<string>("TIPO_ROVER")
                .HasValue<MaquinaAgricola>("MAQUINA_AGRICOLA")
                .HasValue<Drone>("DRONE")
                .HasValue<VeiculoAutonomo>("VEICULO_AUTONOMO");

            // EstacaoBase → Rover (1 para N)
            modelBuilder.Entity<Rover>()
                .HasOne(r => r.EstacaoBase)
                .WithMany()
                .HasForeignKey(r => r.IdEstacaoBase)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario → Rover (1 para N)
            modelBuilder.Entity<Rover>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Rover → SessaoCorrecao (1 para N)
            modelBuilder.Entity<SessaoCorrecao>()
                .HasOne(s => s.Rover)
                .WithMany()
                .HasForeignKey(s => s.IdRover)
                .OnDelete(DeleteBehavior.Restrict);

            // EstacaoBase → SessaoCorrecao (1 para N)
            modelBuilder.Entity<SessaoCorrecao>()
                .HasOne(s => s.EstacaoBase)
                .WithMany()
                .HasForeignKey(s => s.IdEstacaoBase)
                .OnDelete(DeleteBehavior.Restrict);

            // Rover → Ocorrencia (1 para N)
            modelBuilder.Entity<Ocorrencia>()
                .HasOne(o => o.Rover)
                .WithMany()
                .HasForeignKey(o => o.IdRover)
                .OnDelete(DeleteBehavior.Restrict);

            // Precisão decimal LarguraImplemento
            modelBuilder.Entity<MaquinaAgricola>()
                .Property(m => m.LarguraImplemento)
                .HasPrecision(18, 2);
        }
    }
}