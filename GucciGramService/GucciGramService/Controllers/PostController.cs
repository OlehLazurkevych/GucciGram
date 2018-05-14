using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace GucciGramService.Controllers
{
    public class PostController : Controller
    {
        private UserManager<User> userManager;
        private GeneralDbContext generalDbContext;
        private SignInManager<User> signInManager;
        private LikeDbContext likeDbContext;
        private CommentDbContext commentDbContext;

        public PostController(UserManager<User> userManager, GeneralDbContext generalDbContext, SignInManager<User> signInManager, LikeDbContext likeDbContext, CommentDbContext commentDbContext)
        {
            this.userManager = userManager;
            this.generalDbContext = generalDbContext;
            this.signInManager = signInManager;
            this.likeDbContext = likeDbContext;
            this.commentDbContext = commentDbContext;
        }

        [Authorize]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(PostViewModel model)
        {
            User user = null;
            IFormFile uploadedImage = model.files.FirstOrDefault();
            if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream ms = new MemoryStream();
                uploadedImage.OpenReadStream().CopyTo(ms);
                user = await userManager.FindByNameAsync(model.UserID);

                Post post = new Post()
                {
                    PostID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    LikeQuantity = 0,
                    UserID = user.Id,
                    Description = model.Description,
                    Image = ms.ToArray(),
                    ImageType = uploadedImage.ContentType
                };

                generalDbContext.Posts.Add(post);
                generalDbContext.SaveChanges();
            }

            return Redirect("/Home/UserPage/" + user.UserName);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePost(Guid PostID)
        {
            User user = await userManager.FindByNameAsync(this.User.Identity.Name);
            Post post = generalDbContext.Posts.FirstOrDefault(m => m.PostID == PostID);

            bool iterate = true;
            while(iterate)
            {
                PostLike like = likeDbContext.PostLikes.FirstOrDefault(c => c.PostID == post.PostID);
                if(like != null)
                {
                    likeDbContext.PostLikes.Remove(like);
                    likeDbContext.SaveChanges();
                }
                else
                {
                    iterate = false;
                }
            }
            iterate = true;
            while (iterate)
            {
                Comment com = commentDbContext.Comments.FirstOrDefault(c => c.PostID == post.PostID);
                if (com != null)
                {
                    commentDbContext.Comments.Remove(com);
                    commentDbContext.SaveChanges();
                }
                else
                {
                    iterate = false;
                }
            }

            if (post.UserID == user.Id || this.User.IsInRole("Moderator") || this.User.IsInRole("Administrator"))
            {
                generalDbContext.Posts.Remove(post);
                generalDbContext.SaveChanges();
            }
            return Redirect("/Home/Index");
        }
    }
}