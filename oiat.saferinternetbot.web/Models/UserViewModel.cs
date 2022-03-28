using System.ComponentModel.DataAnnotations;

namespace oiat.saferinternetbot.web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail Adresse")]
        public string Email { get; set; }
    }
}