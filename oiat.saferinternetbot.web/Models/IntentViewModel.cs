using System;
using System.ComponentModel.DataAnnotations;

namespace oiat.saferinternetbot.web.Models
{
    public class IntentViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Antworten")]
        public int CountAnswers { get; set; }
    }
}