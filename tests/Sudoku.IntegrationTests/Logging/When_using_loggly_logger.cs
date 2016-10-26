using Microsoft.Extensions.Options;
using Moq;
using Sudoku.Interfaces.Logging;
using Sudoku.Logging;
using Sudoku.Types.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sudoku.IntegrationTests.Logging
{
    public class When_using_loggly_logger
    {
        private readonly Mock<IOptions<Configuration>> _mockOptionsConfiguration;

        public When_using_loggly_logger()
        {
            _mockOptionsConfiguration = new Mock<IOptions<Configuration>>();
            _mockOptionsConfiguration.Setup(o => o.Value)
                .Returns(new Configuration()
                {
                    Logging = new Types.Configuration.Logging()
                    {
                        LogglyApiKey = "fa0efa39-e5e0-413d-80e6-504106af925a",
                        LogglyBaseUrl = "http://logs-01.loggly.com"
                    }
                });
        }

        //Test Subject
        private IAsyncLogger _logger;

        [Fact(Skip = "Logging manual integration test")]
        public async Task Should_log_all_message_types()
        {
            _logger = new LogglyLogger(_mockOptionsConfiguration.Object);

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
