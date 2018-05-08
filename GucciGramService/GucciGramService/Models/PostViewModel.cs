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

        public PostViewModel(Post post, UserManager<User> userManager)
        {
            PostID = post.PostID;
            UserID = post.UserID;
            Image = post.Image;
            ImageType = post.ImageType;
            LikeQuantity = post.LikeQuantity;
            Date = post.Date;
            Description = post.Description;
            this.userManager = userManager;
    }    

        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public IList<IFormFile> files { get; set; }
        
        public async Task<string> GetUserName()
        {
            return (await userManager.FindByIdAsync(UserID)).UserName;
        }
    }
}
