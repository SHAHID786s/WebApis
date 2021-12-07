using System;
using System.Collections.Generic;
using Commander.Data;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase //base means no views :)
    {   
        private readonly ICommanderRepo _repo;
        private readonly ILogger<CommandsController>_log;
        public CommandsController(ICommanderRepo repo, ILogger<CommandsController> log)
        {
            _repo = repo;
            _log = log;
        }

        //GET api/commands/
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands()
        {   
            try{    
                var items=_repo.GetAllCommands();
                _log.LogInformation("Able to get all commands");
                return Ok(items);

            }
            catch (Exception e)
            {
                return BadRequest($"Unable to get all commands {e}");
            }
            
            
        }

        //GET api/commands/5
        [HttpGet("{5}")]
        public ActionResult<Command> GetCommandById(int id)
        {   
            try{
                var obj = _repo.GetCommandById(id);
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest($"unable to find the command{e}");
            }
            
        }
    }
}