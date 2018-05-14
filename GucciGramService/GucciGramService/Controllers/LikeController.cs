using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace GucciGramService.Controllers
{
    public class LikeController : Controller
    {
        private UserManager<User> userManager;
        private GeneralDbContext generalDbContext;
        private LikeDbContext likeDbContext;

        public LikeController(UserManager<User> userManager, GeneralDbContext generalDbContext, LikeDbContext likeDbContext)
        {
            this.userManager = userManager;
            this.generalDbContext = generalDbContext;
            this.likeDbContext = likeDbContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetLike(Guid PostId)
        {
            Post post = generalDbContext.Posts.FirstOrDefault(c => c.PostID == PostId);
            if (post != null)
            {
                User user = await userManager.FindByNameAsync(this.User.Identity.Name);
                if (user != null)
                {
                    PostLike model;
                    
                    model = new PostLike()
                    {
                        PostID = PostId,
                        UserID = user.Id
                    };
                    
                    likeDbContext.Add(model);
                    likeDbContext.SaveChanges();

                    post.LikeQuantity = post.LikeQuantity + 1;
                    await generalDbContext.SaveChangesAsync();
                }
            }
            return Redirect("/Home/");
        }
    }
}