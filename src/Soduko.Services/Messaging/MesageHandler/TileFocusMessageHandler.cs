using Newtonsoft.Json;
using Sudoku.Types.Messages;
using Sudoku.Types.Messages.MessageContentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Services.Messaging.MesageHandler
{
    public class TileFocusMessageHandler : BaseMessageHandler, IMessageHandler
    {
        public TileFocusMessageHandler()
            :base()
        {}

        public async Task HandleMessage(SocketMessage socketMessage)
        {
            var game = GameRepository.GetGame(socketMessage.GameId);

            var message = JsonConvert.DeserializeObject<TileFocusMessage>(socketMessage.Content);

            await game.SendToBothPlayers(new SocketMessage(MessageType.TileFocus, socketMessage.Content, game.Id));

        }
    }
}
