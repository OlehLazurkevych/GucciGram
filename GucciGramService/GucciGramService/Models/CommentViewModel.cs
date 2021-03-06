﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GucciGramService.Models
{
    public class Comment
    {
        public Guid CommentID { get; set; }
        public string UserID { get; set; }
        public Guid PostID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}