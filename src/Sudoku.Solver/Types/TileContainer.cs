using Sudoku.Solver.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Solver.Types
{
    internal class TileContainer : SudokuObserver<TileContainer>, IMomento<TileContainer>
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public IList<int> PossibleValues { get; private set; }
        public int Solution { get; set; }

        private TileContainer _previousState { get; set; }

        #region Constructors
        public TileContainer(int row, int column, int solution)
        {
            Row = row;
            Column = column;
            PossibleValues = null;
            Solution = solution;
        }

        public TileContainer(int row, int column)
            :this(row, column, 0)
        {
            PossibleValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }
        #endregion

        public void RemovePossibleValue(int possibleValue, bool isNotGuess)
        {
            PossibleValues.Remove(possibleValue);

            IsSolved(isNotGuess);
        } 

        public override void Update(TileContainer container, bool unSubscribe)
        {
            if (Solution == 0)
            {
                PossibleValues.Remove(container.Solution);
                IsSolved(unSubscribe);
            }

            if (unSubscribe)
                UnSubscribe(container);
        }

        private void IsSolved(bool shouldUnsubscript)
        {
            if (PossibleValues.Count == 1)
            {
                //Yay we got a solution\
                Solution = PossibleValues.Single();
                PossibleValues = null;

                //Lets update all observers
                CallAllObservers(Solution, shouldUnsubscript);
            }
        }

        #region ICopyable
        public TileContainer Copy()
        {
            TileContainer copy = new TileContainer(Row, Column);
            copy.PossibleValues = new List<int>();

            foreach (var possibleValue in PossibleValues)
                copy.PossibleValues.Add(possibleValue);

            return copy;
        }
        #endregion

        #region IMomento
        public void SaveMomento()
        {
            _previousState = Copy();
        }

        public bool TryRevertToMomento()
        {
            if (_previousState == null)
                return false;

            Row = _previousState.Row;
            Column = _previousState.Column;
            PossibleValues = _previousState.PossibleValues;
            _previousState = null;

            return true;
        }
        #endregion

        #region Private Methods

        public void CallAllObservers(int solution, bool isGuess)
        {
            foreach (var observer in Observers)
                observer.Update(this, isGuess);
        }

        #endregion
    }
}
