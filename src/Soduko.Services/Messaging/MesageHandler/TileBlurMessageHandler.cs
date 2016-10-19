using Newtonsoft.Json;
using Sudoku.Types.Messages;
using Sudoku.Types.Messages.MessageContentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Services.Messaging.MesageHandler
{
    public class TileBlurMessageHandler : BaseMessageHandler, IMessageHandler
    {
        public TileBlurMessageHandler()
            : base()
        { }

        public async Task HandleMessage(SocketMessage socketMessage)
        {
            var game = GameRepository.GetGame(socketMessage.GameId);

            var message = JsonConvert.DeserializeObject<TileBlurMessage>(socketMessage.Content);

            if(message.Value == null)
            {
                await game.SendToBothPlayers(new SocketMessage(MessageType.TileBlur, socketMessage.Content, game.Id));
            }
        }
    }
}
