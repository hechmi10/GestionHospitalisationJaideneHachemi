using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestionHospitalisation.Models;
using Microsoft.AspNetCore.Identity;
using GestionHospitalisation.Data;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace GestionHospitalisation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly GestionHospitalisationContext _context;

    public HomeController(
        GestionHospitalisationContext context,
        ILogger<HomeController> logger,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignIn()
    {
        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole(Role.Admin.ToString())) // Convert to string
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "User");
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(Compte model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Login,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Login);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains(Role.Admin.ToString())) // Convert to string
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "User");
            }
            ModelState.AddModelError(string.Empty, "Invalid credentials");
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(Compte model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new IdentityUser
        {
            UserName = model.Login,
            NormalizedUserName = model.Login.ToUpperInvariant()
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogError("User creation error: {Error}", error.Description);
            }
            return View(model);
        }

        // Get the string representation of the role
        string roleName = model.Role.ToString();

        // Ensure the role exists
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Add user to role
        var roleResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            ModelState.AddModelError("", "Failed to assign user role");
            return View(model);
        }

        // Sign in and redirect - THIS IS THE CRITICAL FIX
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Index", roleName); // Redirect to role-specific area
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}