using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GucciGramService.Models
{
    public class UserPageViewModel
    {
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string Gender { get; set; }
        public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();
    }
}