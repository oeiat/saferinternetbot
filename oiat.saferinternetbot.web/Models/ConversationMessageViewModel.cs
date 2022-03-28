using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oiat.saferinternetbot.Web.Models
{

    public class ConversationMessageViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public MessageType Type { get; set; }
    }

    public enum MessageType
    {
        In,
        Out
    }
}