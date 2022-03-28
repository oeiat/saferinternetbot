using System;

namespace oiat.saferinternetbot.Business.Dtos
{
    public class ScoreDto
    {
        public Guid IntentId { get; set; }
        public double Score { get; set; }
        public static ScoreDto Default => new ScoreDto { IntentId = Guid.Empty };
    }
}
