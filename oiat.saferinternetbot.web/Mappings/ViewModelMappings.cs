using mbit.common.Extensions;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Core;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.saferinternetbot.web.Models;
using oiat.saferinternetbot.Web.Models;
using System;

namespace oiat.saferinternetbot.web.Mappings
{
    public class ViewModelMappings : MappingProfile
    {
        public ViewModelMappings()
        {
            CreateMap<IntentDto, IntentViewModel>();

            CreateMap<AnswerDto, AnswerListItemViewModel>()
                .ForMember(dst => dst.TextTruncated, opt => opt.MapFrom(src => src.Text.Shorten(80, "...")))
                ;

            CreateMap<AnswerDto, AnswerEditViewModel>()
                ;

            CreateMap<AnswerDto, DefaultAnswerEditViewModel>()
                ;

            CreateMap<ApplicationUser, UserViewModel>();

            CreateMap<ApplicationUser, UserCreateViewModel>()
                .IncludeBase<ApplicationUser, UserViewModel>();

            CreateMap<UserViewModel, ApplicationUser>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email))
                ;

            CreateMap<StorageMessageDto, ConversationMessageViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.RowKey))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => Enum.Parse(typeof(Web.Models.MessageType), src.Channel.ToString())));

            CreateMap<TimeControlledMessageViewModel, TimeControlledMessageDto>();
            CreateMap<TimeControlledMessageDto, TimeControlledMessageViewModel>();
        }
    }
}