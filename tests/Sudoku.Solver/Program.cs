using Soduko.Services.Solver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Solver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to sudoku solver");

            //Warm static class up
            //SudokuSolver.TrySolve(new int[9, 9]);

            var totalSolved = 0;

            var times = new List<long>();

            var isSolved = true;

            for (int i = 1; i < 51; i++)
            {
                var sudoku = GetSampleSudoku(i);

                Stopwatch timer = Stopwatch.StartNew();


                isSolved = SudokuSolver.TrySolve(sudoku);

                timer.Stop();

                if (isSolved)
                    totalSolved++;

                Console.WriteLine(string.Format("Complete in : {0}ms. \nSolved : {1}.",
                    timer.ElapsedMilliseconds, isSolved));

                times.Add(timer.ElapsedMilliseconds);

            }

            Console.WriteLine(string.Format("\n\n\nTotal Solved {0}/50. \nAverage Time : {1}ms", totalSolved, times.Average()));

            Console.ReadLine();
        }

        private static int [,] GetSampleSudoku(int number)
        {
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
