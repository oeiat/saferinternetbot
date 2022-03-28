using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oiat.saferinternetbot.web.Models
{
    public class TimeControlledMessageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Start Zeitpunkt")]
        public DateTimeOffset StartTime { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "End Zeitpunkt")]
        public DateTimeOffset EndTime { get; set; }
        [Display(Name = "Aktiv")]
        public bool Enabled { get; set; }
        public IEnumerable<AnswerListItemViewModel> Items { get; set; }
    }
}