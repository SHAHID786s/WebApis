using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class CommanderContext:DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> opt): base(opt) // base calls the dbcontext ctor
        {

        }
        public DbSet<Command> Commands {get;set;}
    }
}