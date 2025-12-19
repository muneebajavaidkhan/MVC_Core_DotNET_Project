using Microsoft.AspNetCore.Mvc;
using SecondProj.Data;
using BCrypt.Net;
using SecondProj.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SecondProj.Controllers
{
    public class AccountController : Controller
    {
        private readonly EcommerceDbContext db;

        public AccountController(EcommerceDbContext context)
        {
            db = context;
        }
        // AccountController.cs ke andar
        public IActionResult CreateAdmin()
        {
            // Check agar admin pehle se database mein hai
            var existingAdmin = db.Registers.FirstOrDefault(u => u.Email == "admin@example.com");

            if (existingAdmin == null)
            {
                var adminUser = new Register
                {
                    Name = "Super Admin",
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // Aapka desired password
                    Role = "Admin"
                };

                db.Registers.Add(adminUser);
                db.SaveChanges();
                return Content("Success: Admin account created with Email: admin@example.com and Password: Admin@123");
            }

            return Content("Admin already exists in database.");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        // Register (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register model)
        {
            ModelState.Remove("Role");
            if (ModelState.IsValid)
            {
                if (db.Registers.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var user = new Register
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = hashedPassword,
                    Role = "User"  
                };
                db.Registers.Add(user);
                db.SaveChanges();
                TempData["Success"] = "Registered successfully. Please login.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // Login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Register model)
        {
            ModelState.Remove("Name");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Role");
            if (ModelState.IsValid)
            {
                var user = db.Registers.FirstOrDefault(u => u.Email == model.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    // Create claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("UserId", user.UserId.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in
                    HttpContext.SignInAsync("CookieAuth", principal);

                    // Session for quick access
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetString("Name", user.Name);

                    TempData["Success"] = "Logged in successfully";

                    // Redirect based on role
                    if (user.Role == "Admin")
                        return RedirectToAction("Index", "Admin");
                    else
                        return RedirectToAction("Index", "User");
                }
                ModelState.AddModelError("", "Invalid email or password");
            }
            return View(model);
        }

    }
}
