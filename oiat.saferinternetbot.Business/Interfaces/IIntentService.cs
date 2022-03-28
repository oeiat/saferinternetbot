using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using oiat.saferinternetbot.Business.Dtos;

namespace oiat.saferinternetbot.Business.Interfaces
{
    public interface IIntentService : IService
    {
        Task<ICollection<IntentDto>> GetAll();
        Task<IntentDto> GetById(Guid id);
        Task<IntentDto> GetByName(string name);
    }
}
