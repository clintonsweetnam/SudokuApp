namespace Sudoku.Interfaces.Solver
{
    public interface ISudokuSolver
    {
        bool Solve(int[,] tiles);
    }
}
