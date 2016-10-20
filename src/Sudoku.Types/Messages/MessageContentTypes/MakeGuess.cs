using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Types.Messages.MessageContentTypes
{
    public class MakeGuessMessage : BaseMessage
    {
        public Guid UserId { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Guess { get; set; }
    }
}
