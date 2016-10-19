using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sudoku.Solver.Interfaces;

namespace Sudoku.Solver.Types
{
    internal abstract class SudokuObserver<T> : ISudokuObservable<T>
    {
        public IDictionary<string, ISudokuObservable<T>> Observers { get; set; }

        public SudokuObserver()
        {
            Observers = new Dictionary<string, ISudokuObservable<T>>();
        }

        public void Subscribe(ISudokuObservable<T> observer)
        {
            if(!Observers.ContainsKey(observer.GetKey()) && !observer.GetKey().Equals(GetKey()))
                Observers.Add(observer.GetKey(), observer);
        }

        public void UnSubscribe(ISudokuObservable<T> observer)
        {
            if (Observers.ContainsKey(observer.GetKey()) && !observer.GetKey().Equals(GetKey()))
                Observers.Remove(observer.GetKey());
        }

        public abstract void Update(T t, bool unSubscribe);

        public abstract string GetKey();

        public abstract bool IsSolved(bool isNotGuess);
    }
}
