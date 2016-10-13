using Soduko.Services.Messaging.MesageHandler;
using Sudoku.Types.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soduko.Services.Messaging
{
    public class MessageHandlerFactory
    {
        public IMessageHandler CreateMessageHandler(MessageType messageType)
        {
            IMessageHandler messageHandler = null;

            switch(messageType)
            {
                case MessageType.Info:
                    messageHandler = new InfoMessageHandler();
                    break;
                case MessageType.Error:
                    messageHandler = new ErrorMessageHandler();
                    break;
                case MessageType.TileFocus:
                    messageHandler = new TileFocusMessageHandler();
                    break;
                case MessageType.TileBlur:
                    messageHandler = new TileBlurMessageHandler();
                    break;
                case MessageType.Connection:
                    throw new Exception("Should not be calling the message handler factory for message of type connection");

            }

            return messageHandler;
        }
    }
}
