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
        public void Should_initialize_tile_conainters_correctly()
        {
            var tileContainers = new TileContainer[9, 9];
                
            _sudokuSolver.InitializeTileConainer(tileContainers, new int [9, 9]);

            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Assert.Equal(9, tileContainers[j, i].PossibleValues.Count);
                    Assert.Equal(0, tileContainers[j, i].Solution);
                    Assert.Equal(20, tileContainers[j, i].Observers.Count);
                }
            }
        }

        [Fact]
        public void Should_set_initial_solutions_correctly_remove_observers()
        {
            var tileContainers = new TileContainer[9, 9];
            var initialSolution = new int[9, 9];
            initialSolution[0, 0] = 1;
            initialSolution[6, 3] = 7;

            _sudokuSolver.InitializeTileConainer(tileContainers, initialSolution);

            Assert.True(AssertIsSolved(tileContainers[0, 0], 1));
            Assert.True(AssertIsSolved(tileContainers[6, 3], 7));
            Assert.Equal(19, tileContainers[1, 0].Observers.Count);
            Assert.Equal(19, tileContainers[2, 2].Observers.Count);
            Assert.Equal(19, tileContainers[6, 8].Observers.Count);
            Assert.Equal(19, tileContainers[7, 3].Observers.Count);
        }

        #region Complete Tests

        [Fact(Skip = "Doesnt work! :)")]
        public void Should_solve_all_test_sudokus()
        {
            for (int i = 1; i < 52; i++)
            {
                var sudoku = GetSampleSudoku(3);

                Assert.True(_sudokuSolver.TrySolve(sudoku));
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


                if (!_sudokuSolver.TrySolve(sudoku))
                    throw new Exception("Oh oh failed to solve one....");

                timer.Stop();

                times.Add(timer.ElapsedMilliseconds);
            }

            Console.WriteLine(string.Format("Average Time : {0}ms", times.Average()));
        }

        #endregion

        #region Private Methods 
        private bool AssertIsSolved(TileContainer tileContainer, int solution)
        {
            return tileContainer.Solution == solution
                && tileContainer.PossibleValues == null
                && tileContainer.Observers == null;
        }

        private int[,] GetSampleSudoku(int number)
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
