using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Core;
using oiat.saferinternetbot.DataAccess.Entities;

namespace oiat.saferinternetbot.Business.Mappings
{
    public class AnswerMapping : MappingProfile
    {
        public AnswerMapping()
        {
            CreateMap<Answer, AnswerDto>();

            CreateMap<DefaultAnswer, AnswerDto>();

            CreateMap<AnswerDto, DefaultAnswer>()
                .ForMember(dst => dst.Id, opt => opt.Ignore());

            CreateMap<AnswerDto, Answer>().ForMember(dst => dst.Id, opt => opt.Ignore());
        }
    }
}
