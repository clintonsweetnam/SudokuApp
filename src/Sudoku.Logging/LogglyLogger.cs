using Microsoft.Extensions.Options;
using Sudoku.Interfaces.Logging;
using Sudoku.Types.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sudoku.Logging
{
    public class LogglyLogger : IAsyncLogger
    {
        private static string _logString = "";
        private readonly Configuration _configuration;
        HttpClient client;

        public LogglyLogger(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
            client = new HttpClient();
        }

        public async Task Trace(string message)
        {
            await WriteMessage($"TRACE : {message};");
        }

        public async Task Debug(string message)
        {
            await WriteMessage($"TRACE : {message};");
        }

        public async Task Info(string message)
        {
            await WriteMessage($"INFO : {message};");
        }

        public async Task Warn(string message)
        {
            await WriteMessage($"WARN : {message};");
        }

        public async Task Warn(string message, Exception ex)
        {
            await WriteMessage($"WARN : {message}; Exception : {ex.Message}; StackTrace : {ex.StackTrace};");
        }

        public async Task Error(string message)
        {
            await WriteMessage($"ERROR : {message};");
        }

        public async Task Error(string message, Exception ex)
        {
            await WriteMessage($"ERROR : {message}; Exception : {ex.Message}; StackTrace : {ex.StackTrace};");
        }

        #region Private Methods
        private async Task WriteMessage(string message)
        {
            string logMessage = $"{DateTime.UtcNow.ToString()} : {message}";

            if (string.IsNullOrEmpty(_logString))
            {
                _logString = logMessage;
            }
            else
            {
                _logString += $"\n{logMessage}";

                if (_logString.Length > 1000)
                {
                    var content = new StringContent(_logString);
                    _logString = "";

                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

                    var url = string.Format("{0}/bulk/{1}/tag/bulk/",
                        _configuration.Logging.LogglyBaseUrl,
                        _configuration.Logging.LogglyApiKey);

                    await client.PostAsync(url, content);
                }


            }
        }

        #endregion
    }
}
