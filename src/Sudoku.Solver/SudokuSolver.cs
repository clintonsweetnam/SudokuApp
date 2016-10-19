using Sudoku.Interfaces.Solver;
using Sudoku.Solver.Types;
using Sudoku.Types.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Solver
{
    public class SudokuSolver : ISudokuSolver
    {
        public bool TrySolve(int[,] tiles)
        {
            var result = false;

            var tileConatiners = new TileContainer[9, 9];

            InitializeTileConainer(tileConatiners, tiles);

            result = IsSolved(tileConatiners);

            if(result)
                return result;

            //OK simple solution didnt work... lets we will have to start guessing
            result = TrySolveByGuessing(tileConatiners);

            return result;
        }

        internal bool TrySolveByGuessing(TileContainer [,] tileConatiners)
        {
            var result = false;

            SaveAllUnsolvedTiles(tileConatiners);

            var guesses = new List<Guess>();

            while(!result)
            {
                try
                {
                    Guess guess = GetGuess(tileConatiners, guesses.Count);
                    guesses.Add(guess);

                    RevertChanges(tileConatiners);
                    ApplyGuessChanges(tileConatiners, guesses);

                    result = IsSolved(tileConatiners);
                }
                catch(NoValidSolutionException)
                {
                    var lastGuess = guesses.OrderByDescending(g => g.GuessOrder).First();

                    while (lastGuess.RemainingPossibleValues.Count != 1)
                    {
                        lastGuess.RemainingPossibleValues.Remove(lastGuess.Value);
                        lastGuess.Value = lastGuess.RemainingPossibleValues.First();

                        try
                        {
                            RevertChanges(tileConatiners);
                            ApplyGuessChanges(tileConatiners, guesses);
                            result = IsSolved(tileConatiners);

                            break;
                        }
                        catch (NoValidSolutionException)
                        {
                            if (guesses.Count == 1)
                                throw new NoValidSolutionException();

                            if (lastGuess.RemainingPossibleValues.Count == 1)
                            {
                                guesses.Remove(lastGuess);
                                lastGuess = guesses.OrderByDescending(g => g.GuessOrder).First();
                            }
                        }
                    }
                }
            }

            return result;
        }

        internal void InitializeTileConainer(TileContainer[,] tileConatiners, int[,] tiles)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    tileConatiners[col, row] = new TileContainer(row, col);
                }
            }

            InitializeObservers(tileConatiners);
            AddInitialSolutions(tileConatiners, tiles);
        }

        #region Private Methods
        private void RevertChanges(TileContainer[,] solutionTiles)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    solutionTiles[col, row].TryRevertToMomento();
                }
            }
        }

        private void ApplyGuessChanges(TileContainer[,] solutionTiles, IList<Guess> guesses)
        {
            foreach (var guess in guesses)
            {
                solutionTiles[guess.Column, guess.Row].SetSolution(guess.Value, false);
            }
        }

        private Guess GetGuess(TileContainer[,] solutionTiles, int guessOrder)
        {
            Guess guess = null;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (solutionTiles[col, row].Solution == 0)
                    {
                        guess = new Guess(solutionTiles[col, row].PossibleValues);
                        guess.Column = col;
                        guess.Row = row;
                        guess.Value = solutionTiles[col, row].PossibleValues.First();
                        guess.GuessOrder = guessOrder;

                        goto end;
                    }
                }
            }

            end:

            return guess;
        }

        private void SaveAllUnsolvedTiles(TileContainer[,] tileConatiners)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if(tileConatiners[col, row].Solution == 0)
                        tileConatiners[col, row].SaveMomento();
                }
            }
        }

        private static bool IsSolved(TileContainer[,] solutionTiles)
        {
            var count = 0;

            foreach (var tile in solutionTiles)
                if (tile.Solution == 0)
                    return false;
                else
                    count += tile.Solution;

            return count == 405;
        }

        private void AddInitialSolutions(TileContainer[,] tileConatiners, int[,] tiles)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (tiles[col, row] != 0)
                        tileConatiners[col, row].SetSolution(tiles[col, row], true);
                }
            }
        }

        private void InitializeObservers(TileContainer[,] tileConatiners)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    AddRowAndColumnObservers(tileConatiners, tileConatiners[col, row]);
                    AddAreaObservers(tileConatiners, tileConatiners[col, row]);
                }
            }
        }

        private void AddAreaObservers(TileContainer[,] tileConatiners, TileContainer tileConatiner)
        {
            var xArea = tileConatiner.Column / 3;
            var yArea = tileConatiner.Row / 3;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    tileConatiner.Subscribe(tileConatiners[(xArea * 3) + i, (yArea * 3) + j]);
                }
            }
        }

        private void AddRowAndColumnObservers(TileContainer[,] tileConatiners, TileContainer tileConatiner)
        {
            for (int k = 0; k < 9; k++)
            {
                tileConatiner.Subscribe(tileConatiners[tileConatiner.Column, k]);
                tileConatiner.Subscribe(tileConatiners[k, tileConatiner.Row]);
            }
        }
        #endregion
    }
}
