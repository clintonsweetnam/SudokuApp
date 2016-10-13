using Sudoku.Types.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soduko.Services.Messaging
{
    public interface IMessageHandler
    {
        Task HandleMessage(SocketMessage socketMessage);
    }
}
