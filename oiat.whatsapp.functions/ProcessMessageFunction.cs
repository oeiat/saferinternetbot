using System;
using System.Threading.Tasks;
using System.Linq;
using Autofac;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.whatsapp.functions.DTOS;
using oiat.whatsapp.functions.Infrastructure;

namespace oiat.whatsapp.functions
{
    public static class ProcessMessageFunction
    {
        [FunctionName("ProcessMessage")]
        public static async Task Run([QueueTrigger("received-messages")] string msg,
            [Table("botmessages")] CloudTable table,
            ILogger log)
        {
            using var scope = DIContainer.Instance.BeginLifetimeScope();
            
            var scoreService = scope.Resolve<IScoreService>();
            var answerService = scope.Resolve<IAnswerService>();
            var client = scope.Resolve<MessengerApiClient>();
            var timeMessageService = scope.Resolve<ITimeControlledMessageService>();

            dynamic data = JsonConvert.DeserializeObject(msg);
            var isTextMessage = !string.IsNullOrEmpty((string)data.payload.text);

            var timestamp = DateTimeOffset.FromUnixTimeSeconds((long)data.payload.timestamp);
            var content = isTextMessage ? (string)data.payload.text : string.Empty;
            var conversationId = (string)data.sender;
            var recipient = (string)data.recipient;

            if (!await TableHelper.CheckCurrenIdenticalMessage(table, conversationId, timestamp, content))
            {
                log.LogWarning($"Message with the same content was already added inthe last {TableHelper.SPAMMESSAGETIMEOUT} seconds");
                return;
            }

            var entity = new StorageMessageDto
            {
                Channel = MessageChannelType.In,
                Content = content,
                ConversationId = conversationId,
                RawContent = msg,
                Timestamp = timestamp,
                Type = GetTypeFromPayload((string)data.payload.type),
                RowKey = data.uuid
            };

            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));

            if (isTextMessage)
            {
                if (content.Length > 500)
                {
                    content = content.Substring(0, 500);
                }

                var score = await scoreService.GetScore(content);
                var answer = await answerService.GetRandomByIntent(score.IntentId);
                await SendAnswer(table, client, answer, recipient, conversationId, log);

            }
            else
            {
                log.LogInformation((string)data.payload.type);
                var answer = await answerService.GetByInvalidMessage();
                await SendAnswer(table, client, answer, recipient, conversationId, log);
            }

            try
            {
                var timeMessages = await timeMessageService.GetMessagesForTimestamp(timestamp);
                if (timeMessages.Any())
                {
                    foreach (var item in timeMessages)
                    {
                        var timeAnswer = await answerService.GetRandomTimeControlledMessage(item.Id);
                        await SendAnswer(table, client, timeAnswer, recipient, conversationId, log);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error while retrieving additional message");
            }
        }

        private static async Task SendAnswer(CloudTable table, MessengerApiClient client, AnswerDto answer, string recipient, string conversationId, ILogger log)
        {
            log.LogInformation(answer.Text);
            var jsonResult = GenerateAnswerJson(answer, recipient, conversationId);
            var messageId = await client.SendMessage(jsonResult);
            await StoreOutMessage(table, messageId, jsonResult);
        }

        private static string GenerateAnswerJson(AnswerDto answer, string recipient, string conversationId)
        {
            var result = new SendMessageDto()
            {
                Identifier = $"{recipient}:{conversationId}",
                Payload = new PayloadDto()
                {
                    Type = "text",
                    Text = answer.Text
                }
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(result, settings);
        }


        private static async Task StoreOutMessage(CloudTable table, string id, string message)
        {
            var sendMsg = JsonConvert.DeserializeObject<SendMessageDto>(message);

            var entity = new StorageMessageDto
            {
                Channel = MessageChannelType.Out,
                Content = sendMsg.Payload.Text,
                ConversationId = sendMsg.Identifier.Split(':', StringSplitOptions.RemoveEmptyEntries)[1],
                RawContent = message,
                RowKey = id,
                Timestamp = DateTimeOffset.UtcNow,
                Type = MessageType.Text
            };
            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        private static MessageType GetTypeFromPayload(string type)
        {
            return type switch
            {
                "text" => MessageType.Text,
                "image" => MessageType.Image,
                "video" => MessageType.Video,
                "audio" => MessageType.Audio,
                "contact" => MessageType.Contact,
                "location" => MessageType.Position,
                _ => MessageType.Unknown,
            };
        }
    }
}
