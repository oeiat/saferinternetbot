using System;
using System.ComponentModel.DataAnnotations;

namespace oiat.saferinternetbot.web.Models
{
    public class AnswerEditViewModel
    {
        public Guid IntentId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Antwort")]
        public string Text { get; set; }
    }
}