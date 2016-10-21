using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sudoku.Types.Messages;
using Sudoku.Repositories;
using Newtonsoft.Json;
using Sudoku.Types.Messages.MessageContentTypes;
using Sudoku.Services.Solver;
using Sudoku.Types.Exceptions;

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
            
            message.IsValidMove = game.Board.IsValidMove(message.Guess, message.XPos, message.YPos);

            if(message.IsValidMove)
            {
                game.Board.Tiles[message.XPos, message.YPos] = message.Guess;
                try
                {
                    message.IsValidMove = SudokuSolver.TrySolve(game.Board.Tiles);
                }
                catch(NoValidSolutionException)
                {
                    message.IsValidMove = false;
                    game.Board.Tiles[message.XPos, message.YPos] = 0;
                }
            }

            await game.SendToBothPlayers(new SocketMessage(MessageType.MakeGuess, JsonConvert.SerializeObject(message), game.Id));
        }
    }
}
