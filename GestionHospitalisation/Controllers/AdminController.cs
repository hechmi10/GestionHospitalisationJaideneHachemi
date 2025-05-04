using GestionHospitalisation.Data;
using GestionHospitalisation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestionHospitalisation.Controllers
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class AdminController : Controller
    {
        private readonly GestionHospitalisationContext _context;

        public AdminController(GestionHospitalisationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Statistiques()
        {
            var hospitalisations = _context.Hospitalisation.ToList();
            var patients = _context.Patient.ToList();
            var services = _context.Service.ToList();

            // Create a ValueTuple with exactly 3 items
            var viewModel = (Hospitalisations: hospitalisations,
                            Patients: patients,
                            Services: services);

            return View(viewModel);
        }
    }
}