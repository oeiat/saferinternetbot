using System;
using System.Collections.Generic;
using System.Text;

namespace oiat.whatsapp.functions.DTOS
{
    public class SendMessageDto
    {
        public string Identifier { get; set; }

        public PayloadDto Payload { get; set; }
    }
}
