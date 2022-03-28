using System;

namespace oiat.saferinternetbot.Business.Dtos
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public Guid IntentId { get; set; }
        public string Intent { get; set; }
        public string Text { get; set; }
    }
}
