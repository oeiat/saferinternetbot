using System.ComponentModel.DataAnnotations;

namespace oiat.saferinternetbot.web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "E-Mail Adresse")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }
    }
}