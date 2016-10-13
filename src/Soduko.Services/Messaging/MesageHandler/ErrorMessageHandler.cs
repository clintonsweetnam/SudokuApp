using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sudoku.Types.Messages;

namespace Soduko.Services.Messaging.MesageHandler
{
    public class ErrorMessageHandler : IMessageHandler
    {
        public Task HandleMessage(SocketMessage socketMessage)
        {
            throw new NotImplementedException();
        }
    }
}
