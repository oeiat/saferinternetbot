using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace oiat.saferinternetbot.Business.Dtos
{
    public class StorageMessageDto : TableEntity
    {
        public const string DEFAULTPARTITIONKEY = "oiat.whatsapp.functions";

        public string ConversationId { get; set; }
        public MessageType Type { get; set; } = MessageType.Text;
        public string Content { get; set; }
        public string MediaLink { get; set; }
        public MessageChannelType Channel { get; set; }
        public string RawContent { get; set; }

        public StorageMessageDto()
            : this(null)
        {

        }

        public StorageMessageDto(string partitionKey)
        {
            PartitionKey = (string.IsNullOrEmpty(partitionKey)) ? "oiat.whatsapp.functions" : partitionKey;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.Azure.Cosmos.Table.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            Channel = (Enum.TryParse<MessageChannelType>(properties[nameof(Channel)].StringValue, true, out var channelType)) ? channelType : MessageChannelType.In;
            Type = (Enum.TryParse<MessageType>(properties[nameof(Type)].StringValue, true, out var messageType)) ? messageType : MessageType.Text;
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.Azure.Cosmos.Table.OperationContext operationContext)
        {
            var result = base.WriteEntity(operationContext);
            result.Add(nameof(Channel), new EntityProperty(Channel.ToString()));
            result.Add(nameof(Type), new EntityProperty(Type.ToString()));
            return result;
        }
    }

    public enum MessageType
    {
        Text = 0,
        Image = 1,
        Video = 2,
        Audio = 3,
        Contact = 4,
        Position = 5,
        Unknown = 99
    }

    public enum MessageChannelType
    {
        In,
        Out
    }
}
