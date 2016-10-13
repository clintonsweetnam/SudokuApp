using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sudoku.Models;

namespace Sudoku.Controllers
{
    [Route("api/game")]
    public class GameController : Controller
    {
        [HttpGet]
        public NewUserModel Get(long? gameId)
        {
            var model = new NewUserModel();
            model.UserId = Guid.NewGuid();

            if (gameId != null)
                model.GameId = (long)gameId;
            else
                model.GameId = DateTime.UtcNow.Millisecond;

            return model;
        }
    }
}
