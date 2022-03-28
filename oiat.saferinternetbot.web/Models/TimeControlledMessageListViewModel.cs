using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace oiat.saferinternetbot.web.Models
{
    public class TimeControlledMessageListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Aktiv")]
        public bool Enabled { get; set; }
    }
}