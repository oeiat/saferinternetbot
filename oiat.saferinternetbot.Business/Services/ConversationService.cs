using AutoMapper;
using mbit.common.cache;
using Microsoft.Azure.Cosmos.Table;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Extensions;
using oiat.saferinternetbot.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.Business.Services
{
    public class ConversationService : IConversationService
    {
        private readonly CloudTable _table;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public ConversationService(IMapper mapper, ICacheService cache, CloudTableClient tableClient)
        {
            _mapper = mapper;
            _cache = cache;
            _table = tableClient.GetTableReference("botmessages");
        }

        public async Task<ICollection<(string conversationId, int msgCount, DateTimeOffset latestTimestamp)>> GetAllConversationIds()
        {
            var query = new TableQuery().Select(new[] { nameof(StorageMessageDto.ConversationId), nameof(StorageMessageDto.Timestamp) });
            var result = (await _table.ExecuteQueryAsync(query)).Select(x => (x.Properties[nameof(StorageMessageDto.ConversationId)].StringValue, x.Timestamp)).GroupBy(x => x.StringValue);
            return result.Select(x => (x.Key, x.Count(), x.Max(s => s.Timestamp))).OrderByDescending(x => x.Item3).ToList();
        }

        public async Task<ICollection<StorageMessageDto>> GetAllMessages()
        {
            var query = new TableQuery<StorageMessageDto>().Where(TableQuery.GenerateFilterCondition(nameof(StorageMessageDto.PartitionKey), QueryComparisons.Equal, StorageMessageDto.DEFAULTPARTITIONKEY));
            var result = await _table.ExecuteQueryAsync(query);
            return result.ToList();
        }

        public async Task<StorageMessageDto> GetById(Guid id)
        {
            var result = await _table.ExecuteAsync(TableOperation.Retrieve<StorageMessageDto>(StorageMessageDto.DEFAULTPARTITIONKEY, id.ToString()));
            return result.Result as StorageMessageDto;
        }

        public async Task<ICollection<StorageMessageDto>> GetConversation(string conversationId)
        {
            var query = new TableQuery<StorageMessageDto>().Where(TableQuery.GenerateFilterCondition(nameof(StorageMessageDto.ConversationId), QueryComparisons.Equal, conversationId));
            var result = await _table.ExecuteQueryAsync(query);
            return result.OrderByDescending(x => x.Timestamp).ToList();
        }

        public async Task DeleteConversation(string conversationId)
        {
            var query = new TableQuery<StorageMessageDto>().Where(TableQuery.GenerateFilterCondition(nameof(StorageMessageDto.ConversationId), QueryComparisons.Equal, conversationId));
            var result = await _table.ExecuteQueryAsync(query);

            var tableBatchOperation = new TableBatchOperation();
            foreach (var row in result)
            {
                tableBatchOperation.Add(TableOperation.Delete(row));
            }
            await _table.ExecuteBatchAsync(tableBatchOperation);
        }

    }
}
