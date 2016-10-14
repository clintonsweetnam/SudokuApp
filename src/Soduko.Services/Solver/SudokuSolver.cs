using Sudoku.Types.Exceptions;
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
        public static int Loops;

        public static bool TrySolve(int[,] tiles)
        {
            var result = false;

            var solutionTiles = InitializesolutionTiles(tiles);

            result = TryBasicSolve(solutionTiles);

            SudokuTile[,] initialTiles = null;

            if (!result)
                initialTiles = CopyTiles(solutionTiles);

            var guesses = new List<Guess>();

            while (!result)
            {
                try
                {
                    guesses.Add(SetNewGuess(solutionTiles, guesses.Count));

                    result = TryBasicSolve(solutionTiles);
                }
                catch(NoValidSolutionException)
                {
                    var lastGuess = guesses.OrderByDescending(g => g.GuessOrder).First();

                    while (lastGuess.RemainingPossibleValues.Count != 1)
                    {
                        lastGuess.RemainingPossibleValues.Remove(lastGuess.Value);
                        lastGuess.Value = lastGuess.RemainingPossibleValues.First();

                        solutionTiles = RevertChangesAndAddGuessChanges(initialTiles, guesses);

                        try
                        {
                            TryBasicSolve(solutionTiles);

                            break;
                        }
                        catch (NoValidSolutionException)
                        {
                            if (guesses.Count == 1)
                                throw new NoValidSolutionException();

                            if(lastGuess.RemainingPossibleValues.Count == 1)
                            {
                                solutionTiles = RevertChangesAndAddGuessChanges(initialTiles, guesses);
                                solutionTiles[lastGuess.Column, lastGuess.Column] = new SudokuTile(lastGuess.SudokuTile);
                                guesses.Remove(lastGuess);

                                lastGuess = guesses.OrderByDescending(g => g.GuessOrder).First();
                            }
                        }
                    }
                }
            }



            return result;
        }

        private static SudokuTile[,] RevertChangesAndAddGuessChanges(SudokuTile[,] initialTiles, IList<Guess> guesses)
        {
            var newTiles = CopyTiles(initialTiles);

            foreach(var guess in guesses)
            {
                newTiles[guess.Column, guess.Row].Solution = guess.Value;
                newTiles[guess.Column, guess.Row].PossibleValues = null;
            }

            return newTiles;
        }

        private static Guess SetNewGuess(SudokuTile[,] solutionTiles, int guessOrder)
        {
            Guess guess = null;

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if(solutionTiles[j, i].PossibleValues != null)
                    {
                        guess = new Guess(solutionTiles[j, i].PossibleValues);
                        guess.Column = j;
                        guess.Row = i;
                        guess.SudokuTile = new SudokuTile(solutionTiles[j, i]);
                        guess.Value = guess.RemainingPossibleValues.First();
                        guess.GuessOrder = guessOrder;

                        solutionTiles[j, i].Solution = guess.Value;
                        solutionTiles[j, i].PossibleValues = null;

                        goto end;
                    }
                }
            }

            end:

            return guess;
        }

        private static SudokuTile[,] CopyTiles(SudokuTile[,] tiles)
        {
            SudokuTile[,] copy = new SudokuTile[9, 9];

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    copy[j, i] = new SudokuTile(tiles[j, i]);
                }
            }

            return copy;
        }

        private static bool TryBasicSolve(SudokuTile[,] solutionTiles)
        {
            var isSolved = false;
            var hasChanged = true;

            while (!isSolved && hasChanged)
            {
                isSolved = true;
                hasChanged = false;

                //Lets see if we have any tiles with only one solution. If so set the solution and nullify possible solution
                hasChanged = RemoveInvalidPossibleValuesAndSetSinglePossibilittes(solutionTiles) ? true : hasChanged;

                hasChanged = SetIfOnlyPossibleSolutionInArea(solutionTiles) ? true : hasChanged;

                hasChanged = SetIfOnlyPossibleSolutionInColumn(solutionTiles) ? true : hasChanged;

                hasChanged = SetIfOnlyPossibleSolutionInRows(solutionTiles) ? true : hasChanged;

                isSolved = IsSolved(solutionTiles);
                Loops++;
            }

            return IsSolved(solutionTiles);
        }

        private static bool IsSolved(SudokuTile[,] solutionTiles)
        {
            foreach (var tile in solutionTiles)
                if (tile.PossibleValues?.Count == 0)
                    throw new NoValidSolutionException();

            foreach (var tile in solutionTiles)
                if (tile.Solution == 0)
                    return false;

            return true;
        }

        private static bool RemoveInvalidPossibleValuesAndSetSinglePossibilittes(SudokuTile[,] solutionTiles)
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (solutionTiles[j, i].Solution != 0 && !solutionTiles[j, i].Processed)
                    {
                        RemovePossibleSolutionsFromRowAndColumn(solutionTiles, solutionTiles[j, i].Solution, i, j);
                        RemovePossibleSolutionsFromArea(solutionTiles, solutionTiles[j, i].Solution, i, j);

                        solutionTiles[j, i].Processed = true;
                        SolvedCount++;
                    }
                }
            }

            return SetSinglePossibilityTiles(solutionTiles);
        }

        private static bool SetIfOnlyPossibleSolutionInRows(SudokuTile[,] solutionTiles)
        {
            bool hasSetSomething = false;

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (solutionTiles[j, i].PossibleValues != null)
                    {
                        var otherPossibleSolutionsInArea = new List<int>();

                        for (var x = 0; x < 9; x++)
                        {
                            if (j != x)
                                if (solutionTiles[x, i].PossibleValues != null)
                                    otherPossibleSolutionsInArea = otherPossibleSolutionsInArea
                                        .Union(solutionTiles[x, i].PossibleValues)
                                        .ToList();
                        }

                        var onlyPossibleSolutions = solutionTiles[j, i].PossibleValues.Except(otherPossibleSolutionsInArea).ToList();

                        if (onlyPossibleSolutions.Count == 1)
                        {
                            //Yay... Got Solution!!
                            solutionTiles[j, i].Solution = onlyPossibleSolutions.Single();
                            solutionTiles[j, i].PossibleValues = null;

                            hasSetSomething = true;
                            RemoveInvalidPossibleValuesAndSetSinglePossibilittes(solutionTiles);
                        }
                    }
                }
            }

            return hasSetSomething;
        }

        private static bool SetIfOnlyPossibleSolutionInColumn(SudokuTile[,] solutionTiles)
        {
            bool hasSetSomething = false;

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (solutionTiles[j, i].PossibleValues != null)
                    {
                        var otherPossibleSolutionsInArea = new List<int>();

                        for (var x = 0; x < 9; x++)
                        {
                            if (i != x)
                                if (solutionTiles[j, x].PossibleValues != null)
                                    otherPossibleSolutionsInArea = otherPossibleSolutionsInArea
                                        .Union(solutionTiles[j, x].PossibleValues)
                                        .ToList();
                        }

                        var onlyPossibleSolutions = solutionTiles[j, i].PossibleValues.Except(otherPossibleSolutionsInArea).ToList();

                        if (onlyPossibleSolutions.Count == 1)
                        {
                            //Yay... Got Solution!!
                            solutionTiles[j, i].Solution = onlyPossibleSolutions.Single();
                            solutionTiles[j, i].PossibleValues = null;

                            hasSetSomething = true;
                            RemoveInvalidPossibleValuesAndSetSinglePossibilittes(solutionTiles);
                        }
                    }
                }
            }

            return hasSetSomething;
        }

        private static bool SetIfOnlyPossibleSolutionInArea(SudokuTile[,] solutionTiles)
        {
            bool hasSetSomething = false;

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (solutionTiles[j, i].PossibleValues != null)
                    {
                        var xArea = j / 3;
                        var yArea = i / 3;

                        var otherPossibleSolutionsInArea = new List<int>();

                        for (var x = 0; x < 3; x++)
                        {
                            for (var y = 0; y < 3; y++)
                            {
                                var thisJ = xArea * 3 + x;
                                var thisI = (yArea * 3) + y;

                                if(thisI != i || thisJ != j)
                                    if (solutionTiles[thisJ, thisI].PossibleValues != null)
                                        otherPossibleSolutionsInArea = otherPossibleSolutionsInArea
                                                .Union(solutionTiles[thisJ, thisI].PossibleValues)
                                                .ToList();
                            }
                        }

                        var onlyPossibleSolutions = solutionTiles[j, i].PossibleValues.Except(otherPossibleSolutionsInArea).ToList();

                        if(onlyPossibleSolutions.Count == 1)
                        {
                            //Yay... Got Solution!!
                            solutionTiles[j, i].Solution = onlyPossibleSolutions.Single();
                            solutionTiles[j, i].PossibleValues = null;

                            hasSetSomething = true;
                            RemoveInvalidPossibleValuesAndSetSinglePossibilittes(solutionTiles);
                        }
                    }
                }
            }

            return hasSetSomething;
        }

        private static bool SetSinglePossibilityTiles(SudokuTile[,] solutionTiles)
        {
            bool hasSetSomething = false;

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (solutionTiles[j, i].Solution == 0 && solutionTiles[j, i].PossibleValues.Count == 1)
                    {
                        solutionTiles[j, i].Solution = solutionTiles[j, i].PossibleValues[0];
                        solutionTiles[j, i].PossibleValues = null;

                        RemoveInvalidPossibleValuesAndSetSinglePossibilittes(solutionTiles);

                        hasSetSomething = true;
                    }
                }
            }

            return hasSetSomething;
        }

        private static void RemovePossibleSolutionsFromArea(SudokuTile[,] solutionTiles, int solution, int row, int column)
        {
            var xArea = column / 3;
            var yArea = row / 3;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (solutionTiles[(xArea * 3) + i, (yArea * 3) + j].PossibleValues != null)
                        solutionTiles[(xArea * 3) + i, (yArea * 3) + j].PossibleValues.Remove(solution);
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

        private class Guess
        {
            public Guess(IList<int> possibleValues)
            {
                RemainingPossibleValues = new List<int>();
                foreach (var possibleValue in possibleValues)
                    RemainingPossibleValues.Add(possibleValue);
            }

            public int Column { get; set; }
            public int Row { get; set; }
            public int Value { get; set; }
            public int GuessOrder { get; set; }
            public SudokuTile SudokuTile { get; set; }
            public IList<int> RemainingPossibleValues { get; set; }
        }

        private class SudokuTile
        {
            public SudokuTile() { }

            public SudokuTile(SudokuTile tile)
            {
                Solution = tile.Solution;
                Processed = tile.Processed;

                if(tile.PossibleValues != null)
                {
                    PossibleValues = new List<int>();
                    foreach (var possibleValue in tile.PossibleValues)
                        PossibleValues.Add(possibleValue);
                }
            }

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
