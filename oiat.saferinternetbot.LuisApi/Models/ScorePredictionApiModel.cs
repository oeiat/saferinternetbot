using System.Runtime.Serialization;

namespace oiat.saferinternetbot.LuisApi.Models
{
    [DataContract]
    public class ScorePredictionApiModel
    {
        [DataMember(Name = "topIntent")]
        public string TopIntent { get; set; }
    }
}