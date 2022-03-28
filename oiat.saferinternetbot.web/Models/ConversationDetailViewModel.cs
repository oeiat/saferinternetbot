using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oiat.saferinternetbot.Web.Models
{
    public class ConversationDetailViewModel
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Konversations-Nummer")]
        public string ConversationId { get; set; }
        [Display(Name = "Anzahl von Nachrichten")]
        public int MessageCount { get; set; }
        [Display(Name = "Nachrichten")]
        public IEnumerable<ConversationMessageViewModel> Messages { get; set; }
    }
}