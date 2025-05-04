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
    }
}