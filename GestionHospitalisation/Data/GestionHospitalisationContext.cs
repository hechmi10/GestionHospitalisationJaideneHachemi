using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionHospitalisation.Models;

namespace GestionHospitalisation.Data
{
    public class GestionHospitalisationContext : DbContext
    {
        public GestionHospitalisationContext (DbContextOptions<GestionHospitalisationContext> options)
            : base(options)
        {
        }

        public DbSet<Compte> Compte { get; set; } = default!;
        public DbSet<Service> Service { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Hospitalisation> Hospitalisation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospitalisation>()
                .HasKey(h => new { h.NumServ, h.CodePat, h.DateEntree });
            base.OnModelCreating(modelBuilder);
        }
    }
}
