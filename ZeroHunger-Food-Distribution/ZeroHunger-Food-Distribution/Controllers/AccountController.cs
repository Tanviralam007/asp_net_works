using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger_Food_Distribution.ViewModels;
using ZeroHunger_Food_Distribution.Models;
using ZeroHunger_Food_Distribution.Helpers;

namespace ZeroHunger_Food_Distribution.Controllers
{
    public class AccountController : Controller
    {
        private ZeroHungerDBEntities db = new ZeroHungerDBEntities();

        // GET: Account/Login
        public ActionResult Login()
        {
            if (SessionHelper.IsLoggedIn())
            {
                return RedirectToDashboard();
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Find user by email and password
                var user = db.Users.FirstOrDefault(u =>
                    u.Email == model.Email &&
                    u.Password == model.Password &&
                    u.IsActive == true);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                // Set session
                SessionHelper.SetUserSession(user.UserId, user.UserRole, user.FullName);

                // Show success message
                TempData["SuccessMessage"] = "Login successful! Welcome " + user.FullName;

                // Redirect based on role
                return RedirectToDashboard();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login: " + ex.Message);
                return View(model);
            }
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            if (SessionHelper.IsLoggedIn())
            {
                return RedirectToDashboard();
            }
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // check if email already exists
                var existingUser = db.Users.FirstOrDefault(u =>
                    u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already exists.");
                    return View(model);
                }

                // create new user
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password, // hash password will be implemented later
                    Phone = model.Phone,
                    UserRole = UserRoles.Restaurant,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                db.Users.Add(user);
                db.SaveChanges();

                // create restaurant profile/record
                var restaurant = new Restaurant
                {
                    UserId = user.UserId,
                    RestaurantName = model.RestaurantName,
                    Address = model.Address,
                    ContactPerson = model.ContactPerson,
                    ContactPhone = model.ContactPhone,
                    Area = model.Area,
                    IsActive = true,
                    JoinedDate = DateTime.Now
                };

                db.Restaurants.Add(restaurant);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful! Please log in.";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later." + ex.Message);
                return View(model);
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            SessionHelper.ClearUserSession();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        // Helper method to redirect to dashboard based on role
        private ActionResult RedirectToDashboard()
        {
            var role = SessionHelper.GetUserRole();

            switch (role)
            {
                case UserRoles.Admin:
                    return RedirectToAction("Dashboard", "Admin");
                case UserRoles.Restaurant:
                    return RedirectToAction("Dashboard", "Restaurant");
                case UserRoles.Employee:
                    return RedirectToAction("Dashboard", "Employee");
                default:
                    SessionHelper.ClearUserSession();
                    TempData["ErrorMessage"] = "Invalid user role. Please log in again.";
                    return RedirectToAction("Login");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}