using Sudoku.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Logging
{
    public class ConsoleLogger : IAsyncLogger
    {
        public Task Trace(string message)
        {
            WriteMessage($"TRACE : {message};", ConsoleColor.Green);

            return Task.CompletedTask;
        }

        public Task Debug(string message)
        {
            WriteMessage($"DEBUG : {message};", ConsoleColor.White);

            return Task.CompletedTask;
        }

        public Task Info(string message)
        {
            WriteMessage($"INFO : {message};", ConsoleColor.Green);

            return Task.CompletedTask;
        }

        public Task Warn(string message)
        {
            WriteMessage($"WARN : {message};", ConsoleColor.Yellow);

            return Task.CompletedTask;
        }

        public Task Warn(string message, Exception ex)
        {
            WriteMessage($"WARN : {message}; Exception : {ex.Message}; StackTrace : {ex.StackTrace};", ConsoleColor.Yellow);

            return Task.CompletedTask;
        }

        public Task Error(string message)
        {
            WriteMessage($"ERROR : {message};", ConsoleColor.Red);

            return Task.CompletedTask;
        }

        public Task Error(string message, Exception ex)
        {
            WriteMessage($"ERROR : {message}; Exception : {ex.Message}; StackTrace : {ex.StackTrace};", ConsoleColor.Red);

            return Task.CompletedTask;
        }

        #region Private Methods
        private void WriteMessage(string message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            string logMessage = $"{DateTime.UtcNow.ToString()} : {message}";

            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
    }
}
