using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Core;
using oiat.saferinternetbot.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.Business.Mappings
{
    public class TimeControlledMessageMapping : MappingProfile
    {
        public TimeControlledMessageMapping()
        {
            CreateMap<TimeControlledMessage, TimeControlledMessageDto>();
            CreateMap<TimeControlledMessageDto, TimeControlledMessage>();
        }
    }
}
