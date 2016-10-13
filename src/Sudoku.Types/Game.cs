using Sudoku.Types.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Types
{
    public class Game
    {
        public long Id { get; private set; }
        public User Player1 { get; private set; }
        public User Player2 { get; private set; }
        public Board Board { get; set; }

        public Game(long id)
        {
            Id = id;
            Board = new Board();
        }

        public void SetPlayer(User user)
        {
            if (Player1 == null)
                Player1 = user;
            else if (Player2 == null)
                Player2 = user;
            else
                throw new Exception("This game already has 2 players");
        }

        public async Task SendToBothPlayers(SocketMessage socketMessage)
        {
            if (Player1 != null)
                await Player1.TrySendMessage(socketMessage);
            if (Player2 != null)
                await Player2.TrySendMessage(socketMessage);
        }
    }
}
