using System.Collections.Generic;

namespace Sudoku.Solver.Interfaces
{
    internal interface ISudokuObservable<T>
    {
        IList<ISudokuObservable<T>> Observers { get; set; }

        void Subscribe(ISudokuObservable<T> observer);

        void UnSubscribe(ISudokuObservable<T> observer);

        void Update(T t, bool unSubscribe);
    }
}
