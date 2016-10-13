
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Sudoku.Types.Messages
{
    public class SocketMessage
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("Type")]
        public MessageType Type { get; private set; }

        [JsonProperty("Content")]
        public string Content { get; private set; }

        public long GameId { get; private set; }

        public SocketMessage(MessageType messageType, string messageContent, long gameId)
        {
            this.Type = messageType;
            this.Content = messageContent;
            this.GameId = gameId;
        }
    }
}
