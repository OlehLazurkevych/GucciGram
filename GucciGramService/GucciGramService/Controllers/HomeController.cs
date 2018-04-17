using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using System.Linq;
using System;

namespace GucciGramService.Controllers
{
    public class HomeController : Controller
    {
        private IUserRepository repository;

        public HomeController(IUserRepository repo)
        {
            repository = repo;
        }

        /*   Action methods   */

        public ViewResult Index()
        {
            // Load main page
            return View("Index");
        }

        [HttpGet]
        public ViewResult LoginForm()
        {
            // Load registration form
            return View();
        }

        [HttpPost]
        public ViewResult LoginForm(LoginFormResponse loginFormResponse)
        {
            if (ModelState.IsValid)
            {
                User user;
                try
                {
                    user = repository.Users.First<User>(p => p.Email == loginFormResponse.Email);
                }
                catch (ArgumentNullException e)
                {
                    return View();
                }

                if (user.Password == loginFormResponse.Password)
                {
                    return View("UserPage", user);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                // Validation error
                return View();
            }
        }

        [HttpGet]
        public ViewResult RegistrationForm()
        {
            // Load registration form
            return View();
        }

        [HttpPost]
        public ViewResult RegistrationForm(RegistrationFormResponse registrationFormResponse)
        {
            if (ModelState.IsValid)
            {
                // Successfully registered
                DBAccess.PushUserFromForm(registrationFormResponse); // Save fresh user to Database
                return View("LoginForm");
            }
            else
            {
                // Validation error
                return View();
            }
        }
    }
}
