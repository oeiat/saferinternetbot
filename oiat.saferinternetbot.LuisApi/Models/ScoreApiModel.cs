using System.Runtime.Serialization;

namespace oiat.saferinternetbot.LuisApi.Models
{
    [DataContract]
    public class ScoreApiModel
    {

        [DataMember(Name = "query")]
        public string Query { get; set; }

        [DataMember(Name = "prediction")]
        public ScorePredictionApiModel Prediction { get; set; }
    }
}
