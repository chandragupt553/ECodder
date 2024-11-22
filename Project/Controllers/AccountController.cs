using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;
using Project.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Project.Controllers
{
    public class AccountController : Controller

    {
        private readonly ProjectContext _context;

        public AccountController(ProjectContext context)
        {
            _context = context;
        }

        // GET: AccountController
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            

            // Create a new user object
            var newUser = new Customer
            {
                UFName = model.UFName,
                ULName = model.ULName,
                MobileNumber = model.MobileNumber,
                UEmail = model.UEmail,
                UPassword = model.UPassword
            };

            // Add the new user to the DbContext and save changes
            _context.Customer.Add(newUser);
            await _context.SaveChangesAsync();

            // Redirect the user to a confirmation page or dashboard
            return RedirectToAction("Loginn", "Account");
        }

        public IActionResult Loginn(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Loginn(Loginn model, string returnUrl)
        {

            // Find the user by email and password
            var user = await _context.Customer.FirstOrDefaultAsync(u => u.UEmail == model.UEmail && u.UPassword == model.UPassword);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password."); // Add error message to ModelState
                return View(model); // Return the view with error message
            }

            // Create claims for user authentication
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UId.ToString()),
                new Claim(ClaimTypes.Name, user.UFName),
                // Add more claims as needed
             };

            // Create authentication ticket
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in user with the created principal
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home"); // Default redirection if returnUrl is not valid
            }
        }

        public async Task<IActionResult> LogOut()
        {
            // Sign out the current user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to a different page after sign out
            return RedirectToAction("Index", "Home");
        }
    }
}