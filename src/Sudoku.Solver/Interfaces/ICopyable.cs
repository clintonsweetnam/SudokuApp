using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Solver.Interfaces
{
    internal interface ICopyable<T>
    {
        T Copy();
    }
}
