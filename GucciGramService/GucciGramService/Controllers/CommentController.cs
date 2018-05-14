using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace GucciGramService.Controllers
{
    public class CommentController : Controller
    {
        private UserManager<User> userManager;
        private GeneralDbContext generalDbContext;
        private LikeDbContext likeDbContext;
        private CommentDbContext commentDbContext;

        public CommentController(UserManager<User> userManager, GeneralDbContext generalDbContext, LikeDbContext likeDbContext, CommentDbContext commentDbContext)
        {
            this.userManager = userManager;
            this.generalDbContext = generalDbContext;
            this.likeDbContext = likeDbContext;
            this.commentDbContext = commentDbContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(Guid PostId, string Description)
        {
            if(Description != "")
            {
                Post post = generalDbContext.Posts.FirstOrDefault(c => c.PostID == PostId);
                if (post != null)
                {
                    User user = await userManager.FindByNameAsync(this.User.Identity.Name);

                    Comment comment = new Comment()
                    {
                        CommentID = Guid.NewGuid(),
                        PostID = post.PostID,
                        UserID = user.Id,
                        Date = DateTime.Now,
                        Description = Description
                    };

                    commentDbContext.Add(comment);
                    commentDbContext.SaveChanges();
                }
            }
            return Redirect("/Home/");
        }
    }
}