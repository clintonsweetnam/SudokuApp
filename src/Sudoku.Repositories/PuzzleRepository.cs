using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Repositories
{
    public class PuzzleRepository
    {
        Random random = new Random();

        public int[,] GetRandomPuzzle()
        {
            int number = random.Next(1, 50);

            var start = (number - 1) * 10 + 1;

            var lines = File.ReadLines("Sample.txt").Skip(start).Take(9).ToList();

            var startSudoku = new int[9, 9];

            for (var i = 0; i < 9; i++)
            {
                char[] chars = lines[i].ToCharArray();
                for (var j = 0; j < 9; j++)
                {
                    startSudoku[j, i] = int.Parse(chars[j].ToString());
                }

            }

            return startSudoku;
        }
    }
}
