using System;
using System.Collections.Generic;

namespace oiat.saferinternetbot.web.Models
{
    public class AnswerListViewModel
    {
        public Guid IntentId { get; set; }

        public string IntentName { get; set; }

        public IEnumerable<AnswerListItemViewModel> Items { get; set; }
    }
}