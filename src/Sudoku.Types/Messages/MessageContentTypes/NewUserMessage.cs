using System;

namespace Sudoku.Types.Messages.MessageContentTypes
{
    public class NewUserMessage : BaseMessage
    {
        public long GameId { get; set; }
        public Guid UserId { get; set; }
    }
}
