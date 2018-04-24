using System.ComponentModel.DataAnnotations;

namespace GucciGramService.Models
{
    public class LoginModel
    {
        [RegularExpression(".+\\@.+\\..+"), Required(ErrorMessage = "Please enter a valid email address")]
        [UIHint("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Your password")]
        [UIHint("password")]
        public string Password { get; set; }
    }

    public class RegistrationModel : LoginModel
    {
        [Required(ErrorMessage = "Please enter Your Name")]
        [UIHint("user name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please specify Your gender")]
        public bool? IsMale { get; set; }
    }

    public class CreateModel : RegistrationModel
    {
        [Required(ErrorMessage = "Please specify a role")]
        public string Role { get; set; }
    }
}
