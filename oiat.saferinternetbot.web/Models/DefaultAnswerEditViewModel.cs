using System;
using System.ComponentModel.DataAnnotations;

namespace oiat.saferinternetbot.web.Models
{
    public class DefaultAnswerEditViewModel
    {
        public int Type { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Antwort")]
        public string Text { get; set; }

        public Guid? TimeControlledMessageId { get; set; }
    }
}