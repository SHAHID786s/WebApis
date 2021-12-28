using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;
using Microsoft.Extensions.Logging;

namespace Commander.Data
{
    public class SqlCommandRepo : ICommanderRepo
    {
        private readonly CommanderContext _ctx;
        private readonly ILogger<SqlCommandRepo> _log;
        public SqlCommandRepo(CommanderContext ctx, ILogger<SqlCommandRepo> log)
        {
            _ctx = ctx;
            _log = log;
        }

        public void CreateCommand(Command command)
        {
            try 
            {       
            
                _ctx.Commands.Add(command);
               
            }
            catch (Exception e)
            {
                _log.LogError($"Unable to save this object: {e}");
            }

        }

        public void DeleteCommand(Command cmd)
        {
            try{
                _ctx.Commands.Remove(cmd);
            }
            catch(Exception e)
            {
                _log.LogError($"unable to delete object {e}");
               
            }
        }

        public IEnumerable<Command> GetAllCommands()
        {
            try
            {
                return _ctx.Commands.ToList();
            }
            catch (Exception e)
            {
                _log.LogError($"Unable to get all objects: {e}");
                return null;
            }

        }

        public Command GetCommandById(int id)
        {
            try
            {
                return _ctx.Commands.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception e)
            {
                _log.LogError($"Unable to find this object: {e}");
                return null;
            }
        }
        public bool SaveChanges()
        
        {
            try
            {
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                {
                    _log.LogInformation($"Unble to save changes: {e}");
                    return false;
                }
            }
        }

        public void UpdateCommand(Command cmd)
        {   
            _ctx.Commands.Update(cmd);
        }
    
    
    
    
    
    
    
    
    
    
    
    }
}