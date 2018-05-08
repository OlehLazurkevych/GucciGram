using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GucciGramService.Controllers
{
    public class SearcheController : Controller
    {
        private UserManager<User> userManager;
        private SearchDbContext searchDbContext;

        public SearcheController(UserManager<User> userManager, SearchDbContext searchDbContext)
        {
            this.userManager = userManager;
            this.searchDbContext = searchDbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Find(string request)
        {
            if (request != null)
            {
                List<User> result = new List<User>();

                string REQ = request.ToUpper();

                foreach (var user in userManager.Users)
                {
                    if (user.NormalizedUserName.Contains(REQ))
                    {
                        result.Add(user);
                    }
                }

                string userID = null;
                if (User.Identity.IsAuthenticated)
                {
                    userID = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
                }

                Search searche = new Search()
                {
                    UserID = userID,
                    SearchText = REQ,
                    Date = DateTime.Now
                };
                searchDbContext.Searches.Add(searche);
                searchDbContext.SaveChanges();

                return View(result);
            }
            return Redirect("/Home/");
        }
    }
}