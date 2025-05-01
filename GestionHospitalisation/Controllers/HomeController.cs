using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestionHospitalisation.Models;
using Microsoft.AspNetCore.Identity;
using GestionHospitalisation.Data;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GestionHospitalisation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly GestionHospitalisationContext _context;

    public HomeController(
        GestionHospitalisationContext context,
        ILogger<HomeController> logger,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            return RedirectToAction("Index", "Admin");
        }
        else if (await _userManager.IsInRoleAsync(user, "User"))
        {
            return RedirectToAction("Index", "User");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignIn(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(Compte model, string returnUrl = null)
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

                // Check roles and redirect
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
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
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Login };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with username {UserName}.", model.Login);

                // Assign default role if needed
                // await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
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