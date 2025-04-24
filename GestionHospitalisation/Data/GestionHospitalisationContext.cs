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

        public DbSet<GestionHospitalisation.Models.Compte> Compte { get; set; } = default!;
        public DbSet<GestionHospitalisation.Models.Service> Service { get; set; }
    }
}
