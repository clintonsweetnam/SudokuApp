using System;

namespace Sudoku.Models
{
    public class NewUserModel
    {
        public long GameId { get; set; }
        public Guid UserId { get; set; }
        public int[,] Board { get; set; }
    }
}
