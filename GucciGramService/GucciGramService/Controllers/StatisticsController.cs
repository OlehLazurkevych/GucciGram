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
    public class StatisticsController : Controller
    {
        private UserManager<User> userManager;
        private GeneralDbContext generalDbContext;
        private LikeDbContext likeDbContext;
        private CommentDbContext commentDbContext;
        private SearchDbContext searchDB;

        public StatisticsController(UserManager<User> userManager, GeneralDbContext generalDbContext, LikeDbContext likeDbContext, CommentDbContext commentDbContext, SearchDbContext searchDB)
        {
            this.userManager = userManager;
            this.generalDbContext = generalDbContext;
            this.likeDbContext = likeDbContext;
            this.commentDbContext = commentDbContext;
            this.searchDB = searchDB;
        }

        public IActionResult Index()
        {
            StatisticsViewModel model = new StatisticsViewModel();

            List<User> femails = new List<User>((from user in userManager.Users
                                         where (user.IsMale == false)
                                         select user).AsEnumerable());
            List<User> mails = new List<User>((from user in userManager.Users
                                         where (user.IsMale == true)
                                         select user).AsEnumerable());
            
            model.FemailQuantity = femails.Count;
            model.MailQuantity = mails.Count;

            List<PostLike> FemLikes = new List<PostLike>((from like in likeDbContext.PostLikes
                                                          where userManager.FindByIdAsync(like.UserID).Result.IsMale == false
                                                          select like).AsEnumerable());

            List<PostLike> MaleLikes = new List<PostLike>((from like in likeDbContext.PostLikes
                                                          where userManager.FindByIdAsync(like.UserID).Result.IsMale == true
                                                          select like).AsEnumerable());

            model.AvarangeLikeQFem = FemLikes.Count;
            model.AvarangeLikeQMail = MaleLikes.Count;

            List<Post> FemComs = new List<Post>((from post in generalDbContext.Posts
                                                          where userManager.FindByIdAsync(post.UserID).Result.IsMale == false
                                                          select post).AsEnumerable());

            List<Post> MaleComs = new List<Post>((from post in generalDbContext.Posts
                                                          where userManager.FindByIdAsync(post.UserID).Result.IsMale == true
                                                           select post).AsEnumerable());

            model.AvarangePostCommentsQFem = FemComs.Count;
            model.AvarangePostCommentsQMail = MaleComs.Count;

            model.Searches = new List<string>((from search in searchDB.Searches
                                               select search.SearchText));

            return View(model);
        }
    }
}