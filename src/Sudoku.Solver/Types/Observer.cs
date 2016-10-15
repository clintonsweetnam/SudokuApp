using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sudoku.Solver.Interfaces;

namespace Sudoku.Solver.Types
{
    internal abstract class SudokuObserver<T> : ISudokuObservable<T>
    {
        public IList<ISudokuObservable<T>> Observers { get; set; }

        public SudokuObserver()
        {
            Observers = new List<ISudokuObservable<T>>();
        }

        public void Subscribe(ISudokuObservable<T> observer)
        {
            Observers.Add(observer);
        }

        public void UnSubscribe(ISudokuObservable<T> observer)
        {
            Observers.Remove(observer);
        }

        public abstract void Update(T t, bool unSubscribe);
    }
}
