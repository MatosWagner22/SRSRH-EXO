using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SRSRH_EXO.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<Capacitacion> Capacitaciones { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<ExperienciaLaboral> ExperienciasLaborales { get; set; }
        public DbSet<Empleado> Empleados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones many-to-many
            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Competencias)
                .WithMany(c => c.Candidatos)
                .UsingEntity(j => j.ToTable("CandidatoCompetencias"));

            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Idiomas)
                .WithMany(i => i.Candidatos)
                .UsingEntity(j => j.ToTable("CandidatoIdiomas"));

            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Capacitaciones)
                .WithMany(c => c.Candidatos)
                .UsingEntity(j => j.ToTable("CandidatoCapacitaciones"));

            // Configuración de relación one-to-many
            modelBuilder.Entity<ExperienciaLaboral>()
                .HasOne(e => e.Candidato)
                .WithMany(c => c.Experiencias)
                .HasForeignKey(e => e.CandidatoId);
        }
    }
}
