using System;

namespace oiat.saferinternetbot.Business.Dtos
{
    public class IntentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CountAnswers { get; set; }
    }
}