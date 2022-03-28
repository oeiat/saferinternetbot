using oiat.saferinternetbot.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.Business.Interfaces
{
    public interface IConversationService : IService
    {
        Task<ICollection<StorageMessageDto>> GetAllMessages();
        Task<ICollection<(string conversationId, int msgCount, DateTimeOffset latestTimestamp)>> GetAllConversationIds();
        Task<StorageMessageDto> GetById(Guid id);
        Task<ICollection<StorageMessageDto>> GetConversation(string conversationId);
        Task DeleteConversation(string conversationId);
    }
}
