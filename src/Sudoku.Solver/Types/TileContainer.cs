using Sudoku.Solver.Interfaces;
using Sudoku.Types.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Solver.Types
{
    internal class TileContainer : SudokuObserver<TileContainer>, IMomento<TileContainer>
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public IList<int> PossibleValues { get; set; }
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

            if (PossibleValues.Count == 0)
                throw new NoValidSolutionException();
        } 

        public void SetSolution(int solution, bool isNotGuess)
        {
            Solution = solution;
            PossibleValues = null;

            CallAllObservers(Solution, isNotGuess);

            IsSolved(isNotGuess);

            if (isNotGuess)
            {
                Observers = null;
                _previousState = null;
            }
        }

        public override bool IsSolved(bool shouldUnsubscript)
        {
            if (PossibleValues != null && PossibleValues.Count == 1)
            {
                //Yay we got a solution\
                Solution = PossibleValues.Single();
                PossibleValues = null;

                //Lets update all observers
                CallAllObservers(Solution, shouldUnsubscript);

                return true;
            }

            return false;
        }

        #region SudokuObserver Overrides
        public override void Update(TileContainer container, bool unSubscribe)
        {
            if (Solution == 0)
            {
                PossibleValues.Remove(container.Solution);

                if (PossibleValues.Count == 0)
                    throw new NoValidSolutionException();
            }

            if (unSubscribe)
                UnSubscribe(container);
        }

        public override string GetKey()
        {
            return $"{Column}-{Row}";
        }
        #endregion

        #region ICopyable
        public TileContainer Copy()
        {
            TileContainer copy = new TileContainer(Row, Column);
            copy.PossibleValues = new List<int>();

            foreach (var possibleValue in PossibleValues)
                copy.PossibleValues.Add(possibleValue);

            copy.Observers = new Dictionary<string, ISudokuObservable<TileContainer>>();

            foreach (var observer in Observers)
                copy.Observers.Add(observer);

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

            PossibleValues = new List<int>();
            foreach (var possibleValue in _previousState.PossibleValues)
                PossibleValues.Add(possibleValue);

            Observers = new Dictionary<string, ISudokuObservable<TileContainer>>();
            foreach (var observer in _previousState.Observers)
                Observers.Add(observer);

            return true;
        }
        #endregion

        #region Private Methods

        private void CallAllObservers(int solution, bool isNotGuess)
        {
            var observers = Observers.Values.ToList();

            foreach (var observer in observers)
            {
                observer.Update(this, isNotGuess);
            }

            foreach (var observer in observers)
            {
                observer.IsSolved(isNotGuess);
            }
        }
        #endregion
    }
}
