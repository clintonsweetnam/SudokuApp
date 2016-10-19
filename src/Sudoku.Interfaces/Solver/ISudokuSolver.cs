namespace Sudoku.Interfaces.Solver
{
    public interface ISudokuSolver
    {
        bool TrySolve(int[,] tiles);
    }
}
