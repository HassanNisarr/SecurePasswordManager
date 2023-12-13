using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurePasswordManager.Models;
using Microsoft.AspNetCore.Http;
using System.Web;


namespace SecurePasswordManager.Controllers
{

    public class AccountController : Controller
    {
        private readonly SecurePasswordManagerContext securePasswordManager;
        public AccountController(SecurePasswordManagerContext securePasswordManager)
        {
            this.securePasswordManager = securePasswordManager;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists
                if (securePasswordManager.Users.Any(u => u.Username == model.Username || u.Email == model.Email))
                {
                    ModelState.AddModelError("Username", "Username or Email already exists.");
                    return View(model);
                }

                // Hashing the password
                var salt = CreateSalt(); // Implement CreateSalt method
                var passwordHash = HashPassword(model.Password, salt); // Implement HashPassword method

                // Creating a new user
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    CreateDate = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow,
                };
                securePasswordManager.Users.Add(user);
                securePasswordManager.SaveChanges();

                // Redirect after successful registration
                return RedirectToAction("Login", "Account"); // or any other page
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // Login action
        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext.Session.GetString("userSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user from the database based on the provided username
                //var user = securePasswordManager.Users.FirstOrDefault(u => u.Username == model.Username);
                var user = securePasswordManager.Users.FirstOrDefault(u => u.Username == model.Username);
                if (user != null)
                {
                    HttpContext.Session.SetString("userSession", model.Username);
                    // Hash the provided password with the user's stored salt
                    var hashedPassword = HashPassword(model.Password, user.Salt);

                    // Compare the hashed password with the one stored in the database
                    if (hashedPassword.SequenceEqual(user.PasswordHash))
                    {
                        // If the passwords match, login is successful
                        // Implement your session or cookie creation logic here

                        // Redirect the user to the desired page after successful login
                        return RedirectToAction("Dashboard", "PasswordRecords"); // Example redirection
                    }
                }
                else
                {
                    ViewBag.Message = "Login Failed...";
                }

                // If user is null or passwords don't match, login has failed
                // Add a generic error message to avoid giving specific reasons for login failure
                ModelState.AddModelError("Username", "Invalid username or password.");
            }

            // Return the same view with the model to display any validation errors or login failure message
            return View(model);
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("userSession") != null)
            {
                HttpContext.Session.Remove("userSession");
                //HttpContext.Session.Clear();

                return RedirectToAction("Login");
            }
            return View();
        }
        private byte[] CreateSalt(int size = 16)
        {
            var buffer = new byte[size];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return buffer;
        }

        private byte[] HashPassword(string password, byte[] salt, int iterations = 10000, int hashByteSize = 20)
        {
            using (var rfc2898DeriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, iterations))
            {
                return rfc2898DeriveBytes.GetBytes(hashByteSize);
            }
        }

    }


}


