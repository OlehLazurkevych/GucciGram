using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;

namespace GucciGramService.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<User> userManager;

        public AdminController(UserManager<User> usrMgr)
        {
            userManager = usrMgr;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}