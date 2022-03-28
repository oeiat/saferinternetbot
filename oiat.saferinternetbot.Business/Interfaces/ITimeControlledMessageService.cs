using oiat.saferinternetbot.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.Business.Interfaces
{
    public interface ITimeControlledMessageService : IService
    {
        Task<TimeControlledMessageDto> GetMessageByID(Guid id);
        Task<IEnumerable<(Guid Id, string Name, bool Enabled)>> GetMessageInfos();
        Task AddMessage(TimeControlledMessageDto message);
        Task UpdateMessage(TimeControlledMessageDto message);
        Task DeleteMessage(Guid id);
        Task<IEnumerable<TimeControlledMessageDto>> GetMessagesForTimestamp(DateTimeOffset timestamp);
    }
}
