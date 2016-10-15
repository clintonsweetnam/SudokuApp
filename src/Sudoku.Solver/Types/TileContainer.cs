using Sudoku.Solver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Solver.Types
{
    internal class TileContainer : ICopyable<TileContainer>
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public IList<int> PossibleValues { get; set; }

        public TileContainer(int row, int column)
        {
            Row = row;
            Column = column;
            PossibleValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        public TileContainer Copy()
        {
            TileContainer copy = new TileContainer(Row, Column);
            copy.PossibleValues = new List<int>();

            foreach (var possibleValue in PossibleValues)
                copy.PossibleValues.Add(possibleValue);

            return copy;
        }
    }
}
