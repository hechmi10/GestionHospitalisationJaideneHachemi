using GestionHospitalisation.Data;
using GestionHospitalisation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GestionHospitalisation.Controllers
{
    [Authorize(Roles = nameof(Role.User))]
    public class UserController : Controller
    {
        private readonly GestionHospitalisationContext _context;
        

        public UserController(GestionHospitalisationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListePatients()
        {
            var context = _context.Patient.ToList();
            return View(context);
        }
        public IActionResult ListeHospitalisations()
        {
            var context=_context.Hospitalisation.Include(x=>x.Service).Include(x=>x.Patient).ToList();
            return View(context);
        }
        public IActionResult ListeServices()
        {
            var context = _context.Service.ToList();
            return View(context);
        }
        [HttpPost]
        public IActionResult ListeHospitalisations(string searchString)
        {
            var hospitalisations = _context.Hospitalisation
                .Include(h => h.Service)
                .Include(h => h.Patient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                hospitalisations = hospitalisations.Where(h =>
                    h.Service.LibServ.Contains(searchString) ||
                    h.Patient.Nom.Contains(searchString) ||
                    h.Patient.Prenom.Contains(searchString) ||
                    h.Frais.ToString().Contains(searchString));
            }

            return View(hospitalisations.ToList());
        }
    }
}