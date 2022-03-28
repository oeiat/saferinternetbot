using System;
using System.Collections.Generic;
using oiat.saferinternetbot.DataAccess.Enums;

namespace oiat.saferinternetbot.web.Models
{
    public class DefaultAnswerListViewModel
    {
        public DefaultAnswerType Type { get; set; }
        public string Name { get; set; }
        public IEnumerable<AnswerListItemViewModel> Items { get; set; }

        public Guid? TimeControlledMessageId { get; set; }
        public IEnumerable<TimeControlledMessageListViewModel> TimeControlledMessages { get; set; }
    }
}