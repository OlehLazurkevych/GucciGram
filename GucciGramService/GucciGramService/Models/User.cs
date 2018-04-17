using Microsoft.AspNetCore.Identity;

namespace GucciGramService.Models
{
    public class User
    {
        public int      UserID      { get; set; }
        public int      RoleID      { get; set; }
        public int      GanderID    { get; set; }
        public string   UserName    { get; set; }
        public string   Bio         { get; set; }
        public string   Email       { get; set; }
        public string   Password    { get; set; }
    }
}
