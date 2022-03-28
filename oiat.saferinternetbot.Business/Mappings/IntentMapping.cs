using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Core;
using oiat.saferinternetbot.LuisApi.Models;

namespace oiat.saferinternetbot.Business.Mappings
{
    public class IntentMapping : MappingProfile
    {
        public IntentMapping()
        {
            CreateMap<IntentApiModel, IntentDto>();
        }
    }
}
