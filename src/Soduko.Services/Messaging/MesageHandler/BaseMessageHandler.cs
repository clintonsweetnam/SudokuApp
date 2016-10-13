using Sudoku.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soduko.Services.Messaging.MesageHandler
{
    public abstract class BaseMessageHandler
    {
        protected readonly GameRepository GameRepository;

        public BaseMessageHandler()
        {
            this.GameRepository = new GameRepository();
        }
    }
}
