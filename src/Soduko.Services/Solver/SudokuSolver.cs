using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soduko.Services.Solver
{
    public static class SudokuSolver
    {
        public static int SolvedCount;

        public static bool TrySolve(int [,] tiles)
        {
            var solutionTiles = InitializesolutionTiles(tiles);

            var isSolved = false;
            var hasChanged = true;

            while(!isSolved && hasChanged)
            {
                hasChanged = RemoveInvalidPossibleValues(solutionTiles);
            }

            return isSolved;
        }

        private static bool RemoveInvalidPossibleValues(SudokuTile[,] solutionTiles)
        {
            var hasChanged = false;

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if(solutionTiles[j, i].Solution != 0 && !solutionTiles[j, i].Processed)
                    {
                        //OK we have a solution.

                        //Lets remove all this from the other items in this row
                        RemovePossibleSolutionsFromRowAndColumn(solutionTiles, solutionTiles[j, i].Solution, i, j);

                        solutionTiles[j, i].Processed = true;
                        SolvedCount++;
                        hasChanged = true;
                    }
                }
            }

            //Lets see if we have any tiles with only one solution. If so set the solution and nullify possible solution
            SetSinglePossibilityTiles(solutionTiles);

            return hasChanged;
        }

        private static void SetSinglePossibilityTiles(SudokuTile[,] solutionTiles)
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (solutionTiles[j, i].Solution == 0 && solutionTiles[j, i].PossibleValues.Count == 1)
                    {
                        solutionTiles[j, i].Solution = solutionTiles[j, i].PossibleValues[0];
                        solutionTiles[j, i].PossibleValues = null;
                    }
                }
            }
        }

        private static void RemovePossibleSolutionsFromRowAndColumn(SudokuTile[,] solutionTiles, int solution, int row, int column)
        {
            for(var i = 0; i < 9; i++)
            {
                if (solutionTiles[i, row].PossibleValues != null)
                    solutionTiles[i, row].PossibleValues.Remove(solution);

                if (solutionTiles[column, i].PossibleValues != null)
                    solutionTiles[column, i].PossibleValues.Remove(solution);
            }
        }

        private static SudokuTile[,] InitializesolutionTiles(int[,] tiles)
        {
            SudokuTile[,] solutionTiles = new SudokuTile[9, 9];

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    solutionTiles[i, j] = new SudokuTile(tiles[i, j]);
                }
            }

            return solutionTiles;
        }

        private class SudokuTile
        {
            public int Solution { get; set; }
            public IList<int> PossibleValues { get; set; }
            public bool Processed { get; set; }

            public SudokuTile(int solution)
            {
                Solution = solution;

                if(Solution == 0)
                    PossibleValues = new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
        }
    }

    
}
