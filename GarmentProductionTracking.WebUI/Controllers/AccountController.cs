using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public AccountController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // ---------- Register ----------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

       [HttpPost]
public async Task<IActionResult> Register(UserRegisterDTO dto)
{
    try
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _userService.RegisterAsync(dto);

        if (!result)
        {
            ModelState.AddModelError("", "User already exists");
            return View(dto);
        }

        return RedirectToAction("Login");
    }
    catch (Exception ex)
    {
        ViewBag.Error = "Error occurred while registering. Please try again.";
        Console.WriteLine("Error during registration: " + ex.Message);
        return View(dto);
    }
}

        // ---------- Login ----------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
public async Task<IActionResult> Login(string username, string password)
{
    try
    {
        var user = await _userService.ValidateUserAsync(username, password);

        if (user == null)
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return user.Role switch
        {
            Domain.Enums.Role.Customer => RedirectToAction("CreateOrder", "Customer"),
            Domain.Enums.Role.Manager => RedirectToAction("Home", "Manager"),
            Domain.Enums.Role.Worker => RedirectToAction("Dashboard", "Worker"),
            _ => RedirectToAction("Login")
        };
    }
    catch (Exception ex)
    {
        // If any error occurs, show a friendly message
        ViewBag.Error = "Something went wrong. Please try again later.";
        
        // Optional: log the actual error message for debugging
        Console.WriteLine("Error during login: " + ex.Message);

        return View();
    }
}
        
// ---------- Logout ----------
       [HttpPost]
public async Task<IActionResult> Logout()
{
    try
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
    catch (Exception ex)
    {
        ViewBag.Error = "Error while logging out. Please try again.";
        Console.WriteLine("Logout error: " + ex.Message);
        return RedirectToAction("Login");
    }
}

        // ---------- Access Denied ----------
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
