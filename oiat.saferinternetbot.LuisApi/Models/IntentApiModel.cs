using System;
using System.Runtime.Serialization;

namespace oiat.saferinternetbot.LuisApi.Models
{
    public class IntentApiModel
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
