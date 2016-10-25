using Sudoku.Interfaces.Logging;
using Sudoku.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sudoku.IntegrationTests.Logging
{
    public class When_using_loggly_logger
    {
        private IAsyncLogger _logger;

        [Fact(Skip = "Logging manual integration test")]
        public async Task Should_log_all_message_types()
        {
            _logger = new LogglyLogger();

            await _logger.Debug("Loggly Debug Message!!");
            await _logger.Trace("Loggly Trace Message!!");
            await _logger.Info("Loggly Info Message!!");
            await _logger.Warn("Loggly Warn Message!!");
            await _logger.Warn("Loggly Warn Message!!", new Exception("Warn Exception"));
            await _logger.Error("Loggly Error Message!!");
            await _logger.Error("Loggly Error Message!!", new Exception("Error Exception"));
        }
    }
}
