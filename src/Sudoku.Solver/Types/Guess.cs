using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Solver.Types
{
    internal class Guess
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
        public IList<int> RemainingPossibleValues { get; set; }
    }
}
