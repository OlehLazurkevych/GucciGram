using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;

namespace GucciGramService.Controllers
{
    public class HomeController : Controller
    {
        /*   Action methods   */

        [AllowAnonymous]
        public ViewResult Index()
        {
            // Load main page
            return View("Index");
        }
    }
}
