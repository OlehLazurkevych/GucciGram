using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GucciGramService.Models
{
    public class CommentViewModel
    {
        public Guid CommentID { get; set; }
        public string UserID { get; set; }
        public Guid PostID { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Comments could not be empty.")]
        [UIHint("Comment")]
        public string Description { get; set; }

        public string UserName { get; set; }
    }
}