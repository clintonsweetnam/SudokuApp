using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Interfaces.Logging
{
    public interface IAsyncLogger
    {
        Task Trace(string message);

        Task Debug(string message);

        Task Info (string message);

        Task Warn(string message);

        Task Warn(string message, Exception ex);

        Task Error(string message);

        Task Error(string message, Exception ex);
    }
}
