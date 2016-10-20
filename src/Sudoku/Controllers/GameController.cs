using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sudoku.Models;
using Sudoku.Repositories;
using Sudoku.Types;

namespace Sudoku.Controllers
{
    [Route("api/game")]
    public class GameController : Controller
    {
        private readonly PuzzleRepository _puzzleRepository;
        private readonly GameRepository _gameRepository;

        public GameController()
        {
            _puzzleRepository = new PuzzleRepository();
            _gameRepository = new GameRepository();
        }

        [HttpGet]
        public NewUserModel Get(long? gameId)
        {
            var model = new NewUserModel();
            model.UserId = Guid.NewGuid();

            if (gameId != null)
            {
                model.GameId = (long)gameId;
                var game = _gameRepository.GetGame(model.GameId);
                model.Board = game.Board.Tiles;
            }
            else
            {
                Game game = new Game(DateTime.UtcNow.Millisecond);
                game.Board.Tiles = _puzzleRepository.GetRandomPuzzle();
                _gameRepository.AddGame(game);

                model.GameId = game.Id;
                model.Board = game.Board.Tiles;
            }

            

            return model;
        }
    }
}
