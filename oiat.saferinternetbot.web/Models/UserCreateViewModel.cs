using System.ComponentModel.DataAnnotations;

namespace oiat.saferinternetbot.web.Models
{
    public class UserCreateViewModel : UserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Passwort bestätigen")]
        public string ConfirmPassword { get; set; }
    }
}