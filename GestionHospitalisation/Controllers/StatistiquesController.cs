using GestionHospitalisation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionHospitalisation.Controllers
{
    public class StatistiquesController : Controller
    {
        private readonly GestionHospitalisationContext _context;

        public StatistiquesController(GestionHospitalisationContext context)
        {
            _context = context;
        }
        
    }
}
