using Sudoku.Solver;
using Sudoku.Solver.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Sudoku.UnitTests.Solver
{
    public class When_using_SudokuSolver
    {

        //Test Subject
        private readonly SudokuSolver _sudokuSolver;

        public When_using_SudokuSolver()
        {
            _sudokuSolver = new SudokuSolver();
        }

        [Fact]
        public void Should_solve_all_test_sudokus()
        {
            for (int i = 1; i < 52; i++)
            {
                var sudoku = GetSampleSudoku(i);

                Assert.True(_sudokuSolver.Solve(sudoku));
            }
        }

        [Fact(Skip = "User for Performance Testing")]
        public void Performance_test_SudokuSolver()
        {
            var times = new List<long>();

            for (int i = 1; i < 52; i++)
            {
                var sudoku = GetSampleSudoku(i);

                Stopwatch timer = Stopwatch.StartNew();


                if (!_sudokuSolver.Solve(sudoku))
                    throw new Exception("Oh oh failed to solve one....");

                timer.Stop();

                times.Add(timer.ElapsedMilliseconds);
            }

            Console.WriteLine(string.Format("Average Time : {0}ms", times.Average()));
        }

        #region Private Methods 
        private static int[,] GetSampleSudoku(int number)
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
        #endregion
    }
}
