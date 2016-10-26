using Microsoft.AspNetCore.Mvc;
using Sudoku.Types.Configuration;
using Microsoft.Extensions.Options;
using Sudoku.Interfaces.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sudoku.Controllers
{
    public class HomeController : Controller
    {
        private readonly Configuration _configuration;
        private readonly IAsyncLogger _logger;

        public HomeController(IOptions<Configuration> configuration, IAsyncLogger logger)
        {
            _configuration = configuration.Value;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            Stopwatch timer = Stopwatch.StartNew();

            await Task.Delay(10);

            timer.Stop();

            await _logger.Trace($"Time in HomeController : {timer.ElapsedMilliseconds}ms");

            return View();
        }
    }
}
