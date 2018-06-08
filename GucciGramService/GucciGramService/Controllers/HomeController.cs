using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GucciGramService.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> userManager;
        private GeneralDbContext generalDbContext;
        private LikeDbContext likeDbContext;
        private CommentDbContext commentDbContext;

        public HomeController(UserManager<User> userManager, GeneralDbContext generalDbContext, LikeDbContext likeDbContext, CommentDbContext commentDbContext)
        {
            this.userManager = userManager;
            this.generalDbContext = generalDbContext;
            this.likeDbContext = likeDbContext;
            this.commentDbContext = commentDbContext;
        }

        /*   Action methods   */

        [AllowAnonymous]
        public ViewResult Index()
        {
            IEnumerable<Post> dbresult = (from post in generalDbContext.Posts
                                          orderby post.LikeQuantity
                                          select post).Take(10);

            List<PostViewModel> result = new List<PostViewModel>();
            foreach(var post in dbresult)
            {
                result.Add(new PostViewModel(post, userManager, generalDbContext, likeDbContext, commentDbContext));
            }
            result.Reverse();

            return View(result);
        }

        [AllowAnonymous]
        public async Task<IActionResult> UserPage(string id)
        {
            UserPageViewModel result = new UserPageViewModel();
            User user = await userManager.FindByNameAsync(id);
            if(user == null)
            {
                user = await userManager.FindByIdAsync(id);
            }
            if(user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            result.UserName = user.UserName;
            result.Bio = user.Bio;

            foreach(var post in generalDbContext.Posts)
            {
                if(post.UserID == user.Id)
                {
                    result.Posts.Add(new PostViewModel(post, userManager, generalDbContext, likeDbContext, commentDbContext));
                }
            }

            return View(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public FileStreamResult ViewImage(Guid id)
        {
            Post post = generalDbContext.Posts.FirstOrDefault(m => m.PostID == id);

            MemoryStream ms = new MemoryStream(post.Image);

            return new FileStreamResult(ms, post.ImageType);
        }
    }
}
