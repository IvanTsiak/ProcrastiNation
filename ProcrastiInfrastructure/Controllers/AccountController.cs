using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure;
using System.Security.Claims;

namespace ProcrastiInfrastructure.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProcrastiContext _context;

        public AccountController(ProcrastiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Ти чого не все заповнив? Ану бігом, щоб усе було заповнене!");
                return View();
            }

            bool emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            if (emailExists)
            {
                ModelState.AddModelError("", "Ця пошта уже використовується.");
                return View();
            }

            bool usernameExists = await _context.Users.AnyAsync(u => u.Username == username);
            if (usernameExists)
            {
                ModelState.AddModelError("", "Цей нікнейм уже використовується.");
                return View();
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Username = username,
                Email = email,
                Passwordhash = passwordHash,
                Joineddate = DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Ти чого не все заповнив? Ану бігом, щоб усе було заповнене!");
                return View();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null || BCrypt.Net.BCrypt.Verify(password, user.Passwordhash))
            {
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Невірна пошта або пароль.");

            return View();
        }
    }
}
