using System.Collections.Generic;

namespace Sudoku.Solver.Interfaces
{
    internal interface ISudokuObservable<T>
    {
        IDictionary<string, ISudokuObservable<T>> Observers { get; set; }

        void Subscribe(ISudokuObservable<T> observer);

        void UnSubscribe(ISudokuObservable<T> observer);

        void Update(T t, bool unSubscribe);

        bool IsSolved(bool isNotGuess);

        string GetKey();
    }
}
