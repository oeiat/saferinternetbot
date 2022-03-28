using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oiat.saferinternetbot.Web.Models
{
    public class ConversationOverviewViewModel
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Konversations-Nummer")]
        public string ConversationId { get; set; }
        [Display(Name = "Anzahl von Nachrichten")]
        public int MessageCount { get; set; }
        [Display(Name = "Letzte Nachricht")]
        public DateTimeOffset LatestTimestamp { get; set; }
    }
}