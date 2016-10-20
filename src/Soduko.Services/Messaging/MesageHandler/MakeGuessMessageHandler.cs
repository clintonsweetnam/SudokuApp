using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sudoku.Types.Messages;
using Sudoku.Repositories;
using Newtonsoft.Json;
using Sudoku.Types.Messages.MessageContentTypes;

namespace Sudoku.Services.Messaging.MesageHandler
{
    public class MakeGuessMessageHandler : BaseMessageHandler, IMessageHandler
    {
        public MakeGuessMessageHandler()
            : base()
        { }

        public async Task HandleMessage(SocketMessage socketMessage)
        {
            var game = GameRepository.GetGame(socketMessage.GameId);

            var message = JsonConvert.DeserializeObject<MakeGuessMessage>(socketMessage.Content);

            var validGuess = game.Board.IsValidMove(message.Guess, message.XPos, message.YPos);

        }
    }
}
