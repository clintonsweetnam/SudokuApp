using Sudoku.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Repositories
{
    public class GameRepository
    {
        private static IDictionary<long, Game> Games { get; set; }

        static GameRepository()
        {
            Games = new Dictionary<long, Game>();
        }

        public GameRepository()
        {
        }

        public void AddGame(Game game)
        {
            Games.Add(game.Id, game);
        }

        public Game GetGame(long Id)
        {
            Game game = null;

            if (Games.ContainsKey(Id))
                game = Games[Id];

            return game;
        }

        public void UpdateGame(Game game, long Id)
        {
            Games[Id] = game;
        }

        public void DeleteGame(long Id)
        {
            if (Games.ContainsKey(Id))
                Games.Remove(Id);
        }
    }
}
