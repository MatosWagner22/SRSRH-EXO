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
            // Configuración de relaciones
            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Experiencias)
                .WithOne(e => e.Candidato)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
