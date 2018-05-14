using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GucciGramService.Models
{
    public class PostLike
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public Guid PostID { get; set; }
    }
}
