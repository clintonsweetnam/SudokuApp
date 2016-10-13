using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Types.Messages.MessageContentTypes
{
    public class GameReadyToStartMessage : BaseMessage
    {
        public int StartInSeconds { get; set; }

        public GameReadyToStartMessage(int startInSeconds)
        {
            this.StartInSeconds = startInSeconds;
        }
    }
}
