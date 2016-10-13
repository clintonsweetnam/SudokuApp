using Soduko.Services.Solver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Solver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to sudoku solver");

            SudokuSolver.TrySolve(new int[9, 9]);

            Stopwatch timer = Stopwatch.StartNew();

            var startSudoku = new int[9, 9];
            startSudoku[1, 0] = 3;
            startSudoku[3, 0] = 9;
            startSudoku[7, 0] = 2;

            startSudoku[0, 1] = 8;
            startSudoku[5, 1] = 2;
            startSudoku[8, 1] = 7;

            startSudoku[2, 2] = 1;
            startSudoku[3, 2] = 4;
            startSudoku[6, 2] = 6;

            startSudoku[1, 3] = 9;
            startSudoku[4, 3] = 4;
            startSudoku[6, 3] = 5;
            startSudoku[8, 3] = 2;

            startSudoku[3, 4] = 6;
            startSudoku[5, 4] = 3;

            startSudoku[0, 5] = 7;
            startSudoku[2, 5] = 6;
            startSudoku[4, 5] = 1;
            startSudoku[7, 5] = 8;

            startSudoku[2, 6] = 9;
            startSudoku[5, 6] = 4;
            startSudoku[6, 6] = 1;

            startSudoku[0, 7] = 2;
            startSudoku[3, 7] = 8;
            startSudoku[8, 7] = 3;

            startSudoku[2, 8] = 7;
            startSudoku[5, 8] = 9;
            startSudoku[7, 8] = 5;

            var isSolved = SudokuSolver.TrySolve(startSudoku);

            timer.Stop();

            Console.WriteLine(string.Format("Complete in : {0}ms. \nResult : {1}. \nNumber Solved : {2}.", timer.ElapsedMilliseconds, isSolved, SudokuSolver.SolvedCount));

            Console.ReadLine();
        }
    }
}
