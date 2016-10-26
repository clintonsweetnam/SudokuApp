using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sudoku.Types.Configuration;
using Microsoft.Extensions.Options;
using Sudoku.Interfaces.Logging;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}
