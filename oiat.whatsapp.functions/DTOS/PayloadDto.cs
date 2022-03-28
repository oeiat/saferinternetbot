using System;
using System.Collections.Generic;
using System.Text;

namespace oiat.whatsapp.functions.DTOS
{
    public class PayloadDto
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Attachment { get; set; }
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
    }
}
