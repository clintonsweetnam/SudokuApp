using System;

namespace Sudoku.Types.Messages.MessageContentTypes
{
    public class TileFocusMessage : BaseMessage
    {
        public Guid UserId { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
    }
}
