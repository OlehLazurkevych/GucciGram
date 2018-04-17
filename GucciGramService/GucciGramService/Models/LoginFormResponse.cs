using System.ComponentModel.DataAnnotations;

namespace GucciGramService.Models
{
    public class LoginFormResponse
    {
        [RegularExpression(".+\\@.+\\..+"), Required(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
    }
}
