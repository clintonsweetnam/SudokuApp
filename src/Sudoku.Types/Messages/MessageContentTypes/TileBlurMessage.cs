using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Types.Messages.MessageContentTypes
{
    public class TileBlurMessage : TileFocusMessage
    {
        public int? Value { get; set; }
    }
}
