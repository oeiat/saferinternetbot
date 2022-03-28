using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.DataAccess.Enums;

namespace oiat.saferinternetbot.Business.Interfaces
{
    public interface IAnswerService : IService
    {
        Task<AnswerDto> GetRandomByIntent(Guid intentId);
        Task<AnswerDto> GetByInvalidMessage();
        Task<ICollection<AnswerDto>> GetByIntentId(Guid intentId);
        Task<AnswerDto> GetById(Guid id);
        Task<AnswerDto> Add(AnswerDto model);
        Task<AnswerDto> Update(Guid answerId, AnswerDto model);
        Task Delete(Guid answerId);

        Task<ICollection<AnswerDto>> GetByType(DefaultAnswerType type);
        Task<ICollection<AnswerDto>> GetByType(DefaultAnswerType type, Guid id);
        Task<AnswerDto> GetDefaultById(Guid id);
        Task<AnswerDto> AddDefault(DefaultAnswerType type, AnswerDto model, Guid? timeControlledMessageId = null);
        Task<AnswerDto> UpdateDefault(Guid answerId, AnswerDto model);
        Task DeleteDefault(Guid answerId);
        Task<AnswerDto> GetRandomTimeControlledMessage(Guid timeControlledMessageId);
    }
}