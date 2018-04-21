using Microsoft.AspNetCore.Identity;

namespace GucciGramService.Models
{
    public class User : IdentityUser
    {
        public bool     IsMale      { get; set; }
        public string   Bio         { get; set; }
    }
}