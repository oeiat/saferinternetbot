using Microsoft.Azure.Cosmos.Table;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oiat.whatsapp.functions.Infrastructure
{
    public static class TableHelper
    {
        public const int SPAMMESSAGETIMEOUT = 15;

        public static async Task<bool> CheckCurrenIdenticalMessage(CloudTable table, string conversationId, DateTimeOffset timestamp, string content)
        {
            TableQuery<StorageMessageDto> query = new TableQuery<StorageMessageDto>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition(nameof(StorageMessageDto.ConversationId), QueryComparisons.Equal, conversationId),
                        TableOperators.And,
                        TableQuery.GenerateFilterConditionForDate(nameof(StorageMessageDto.Timestamp), QueryComparisons.GreaterThanOrEqual, timestamp.AddSeconds(-SPAMMESSAGETIMEOUT))
                        ));
            var result = await table.ExecuteQueryAsync(query);

            return result.All(x => x.Content != content);
        }

        public static async Task<bool> CheckCurrenIdenticalMessage(CloudTable table, dynamic data)
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds((long)data.payload.timestamp);
            var content = (string)data.payload.text;
            var conversationId = (string)data.sender;

            return await CheckCurrenIdenticalMessage(table, conversationId, timestamp, content);
        }
    }
}
