using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GucciGramService.Models
{
    public class StatisticsViewModel
    {
        public int FemailQuantity { get; set; }
        public int MailQuantity { get; set; }
        public int FemailPostQuantity { get; set; }
        public int MailPostQuantity { get; set; }
        public int AvarangePostCommentsQFem { get; set; }
        public int AvarangePostCommentsQMail { get; set; }
        public int AvarangeLikeQFem { get; set; }
        public int AvarangeLikeQMail { get; set; }
        public List<string> Searches { get; set; }
    }
}
