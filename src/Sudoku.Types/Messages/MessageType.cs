using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Types.Messages
{
    public enum MessageType
    {
        GameReady,
        Info,
        Connection,
        Error,
        TileFocus,
        TileBlur
    }
}
