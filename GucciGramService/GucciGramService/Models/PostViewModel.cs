using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GucciGramService.Models
{
    public class Post
    {
        public Guid PostID { get; set; }
        public string UserID { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public int LikeQuantity { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class PostViewModel : Post
    {
        private UserManager<User> userManager;
        private GeneralDbContext generalDbContext;
        private LikeDbContext likeDbContext;
        private CommentDbContext commentDbContext;

        public PostViewModel()
        {
            // Empty
        }

        public PostViewModel(Post post)
        {
            PostID = post.PostID;
            UserID = post.UserID;
            Image = post.Image;
            ImageType = post.ImageType;
            LikeQuantity = post.LikeQuantity;
            Date = post.Date;
            Description = post.Description;
        }

        public PostViewModel(Post post, UserManager<User> userManager, GeneralDbContext generalDbContext, LikeDbContext likeDbContext, CommentDbContext commentDbContext)
        {
            PostID = post.PostID;
            UserID = post.UserID;
            Image = post.Image;
            ImageType = post.ImageType;
            LikeQuantity = post.LikeQuantity;
            Date = post.Date;
            Description = post.Description;
            this.userManager = userManager;
            this.generalDbContext = generalDbContext;
            this.likeDbContext = likeDbContext;
            this.commentDbContext = commentDbContext;
            
            IEnumerable<Comment> dbresult = (from comment in commentDbContext.Comments
                                             where comment.PostID == PostID
                                             select comment);
            Comments = new List<Comment>(dbresult);
        }    

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public IList<IFormFile> files { get; set; }
        
        public async Task<string> GetUserName()
        {
            return (await userManager.FindByIdAsync(UserID)).UserName;
        }

        public async Task<bool> LikedByUser(string UserName)
        {
            User user = await userManager.FindByNameAsync(UserName);
            if (user != null)
            {
                PostLike model = likeDbContext.PostLikes.FirstOrDefault(c => c.PostID == PostID && c.UserID == user.Id);

                if (model != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
