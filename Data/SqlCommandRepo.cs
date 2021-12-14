using System.Collections.Generic;
using System.Linq;
using Commander.Models;
using Microsoft.Extensions.Logging;

namespace Commander.Data
{
    public class SqlCommandRepo : ICommanderRepo
    {   
        private readonly CommanderContext _ctx;
        private readonly ILogger <SqlCommandRepo>_log;
        public SqlCommandRepo(CommanderContext ctx, ILogger<SqlCommandRepo> log)
        {
            _ctx=ctx;
            _log = log;
        }
        public IEnumerable<Command> GetAllCommands()
        {
            return _ctx.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _ctx.Commands.FirstOrDefault(x => x.Id == id);
        }
    }
}