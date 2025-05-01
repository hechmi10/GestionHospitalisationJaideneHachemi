using GestionHospitalisation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GestionHospitalisation.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        

        public UserController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}