using GestionHospitalisation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionHospitalisation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        

        public AdminController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}