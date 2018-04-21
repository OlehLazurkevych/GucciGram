using System.ComponentModel.DataAnnotations;

namespace GucciGramService.Models
{
    public class RegistrationFormModel
    {
        [Required(ErrorMessage = "Please enter Your Name")]
        public string UserName { get; set; }

        [RegularExpression(".+\\@.+\\..+"), Required(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please specify an option")]
        public bool? IsMale { get; set; }
    }

    public class LoginFormModel
    {
        [RegularExpression(".+\\@.+\\..+"), Required(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
    }
}
