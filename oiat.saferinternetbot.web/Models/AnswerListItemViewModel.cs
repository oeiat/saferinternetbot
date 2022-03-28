using System;
using System.ComponentModel.DataAnnotations;

namespace oiat.saferinternetbot.web.Models
{
    public class AnswerListItemViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Text (gekürzt)")]
        public string TextTruncated { get; set; }
    }
}