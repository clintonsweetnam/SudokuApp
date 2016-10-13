using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sudoku.Types.Messages;
using Sudoku.Repositories;

namespace Soduko.Services.Messaging.MesageHandler
{
    public class InfoMessageHandler : BaseMessageHandler, IMessageHandler
    {
        public async Task HandleMessage(SocketMessage socketMessage)
        {
            var game = GameRepository.GetGame(socketMessage.GameId);

            await game.SendToBothPlayers(new SocketMessage(MessageType.Info, socketMessage.Content, game.Id));
        }
    }
}
