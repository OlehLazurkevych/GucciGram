using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using System.Linq;
using System;

namespace GucciGramService.Controllers
{
    public class HomeController : Controller
    {
        /*   Action methods   */

        public ViewResult Index()
        {
            // Load main page
            return View("Index");
        }
    }
}
