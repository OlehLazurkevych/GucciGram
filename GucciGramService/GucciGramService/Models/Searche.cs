using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GucciGramService.Models
{
    public class Search
    {
        public int SearchID { get; set; }
        public string UserID { get; set; }
        public string SearchText { get; set; }
        public DateTime Date { get; set; }
    }
}
