using System;
using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase //base means no views :)
    {
        private readonly ICommanderRepo _repo;
        private readonly IMapper _map;
        private readonly ILogger<CommandsController> _log;
        public CommandsController(ICommanderRepo repo, ILogger<CommandsController> log, IMapper map)
        {
            _repo = repo;
            _log = log;
            _map = map;
        }

        //GET api/commands/
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            try
            {
                var items = _repo.GetAllCommands();
                _log.LogInformation("Able to get all commands");
                return Ok(_map.Map<IEnumerable<CommandReadDto>>(items));

            }
            catch (Exception e)
            {
                return BadRequest($"Unable to get all commands {e}");
            }

        }

        //GET api/commands/5
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)

        {
            try
            {
                var obj = _repo.GetCommandById(id);
                return Ok(_map.Map<CommandReadDto>(obj)); // here we filter
                                                          // the command obj from the db into a CommandReadDto object 
                                                          //which doesnt contain the platform prop
            }
            catch (Exception e)
            {
                return NotFound($"unable to find the command{e}");
            }
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto createDto)
        {
            try
            {
                var createModel = _map.Map<Command>(createDto); //mapping the source which is in the brackets(createDto) obj to the destination
                                                                // object which is command
                                                                // which is a table inside the db
                _repo.CreateCommand(createModel);
                _repo.SaveChanges();
                // read from dto so we dont have to see the full object only the props we are interested in
                var commandRDto = _map.Map<CommandReadDto>(createModel); // transfer newley created command which is var 
                                                                         //createModel into a commandreaddto for viewing purposes 
                var routeValues = new { Id = commandRDto.Id };
                return CreatedAtRoute("GetCommandById", routeValues, commandRDto);
            }
            catch (Exception e)
            {
                return BadRequest($"Unable create resource {e}");

            }
        }

        //Put api/commands/{id}

        [HttpPut("{id}")] // not used alot because it loads all of the 
                          //object attributes even the ones we don't change
                          // therefore inefficient use patch instead
        public ActionResult UpdateCommand(int id, CommandUpdateDto cmdUpdateDto)
        {

            var cmdModelFromRepo = _repo.GetCommandById(id);
            if (cmdModelFromRepo == null)
            {
                return NotFound();
            }

            _map.Map(cmdUpdateDto, cmdModelFromRepo);// even though this updates command object we will use the UpdateCommand() function for best practise
            _repo.UpdateCommand(cmdModelFromRepo);
            _repo.SaveChanges();
            //Console.WriteLine("successfully dsved changes");
            return NoContent();


        }

        //api/commands/{id} // need newtonsoft and json patch
        //in postman use the following json object to patch obj
        /*
        [
            {
                "op":"replace",
                "path":"/howto",
                "value":"Hot reload server"
            }
        ]
        */
        [HttpPatch("{id}")] //partial updates not fully re writing whole object
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {   
            
            var getCommandModelObj = _repo.GetCommandById(id);// ge the full cmd object
            if (getCommandModelObj == null)
            {
                return NotFound();
            }

            var cmdToPatch = _map.Map<CommandUpdateDto>(getCommandModelObj);// now map the cmd object to an dto 
                                                                            //one (only interested in certain fields for obj e.g.
                                                                            // no id field, platform is given to client)

            patchDoc.ApplyTo(cmdToPatch, ModelState); // modelstate is ensuring validity of data.
            if (!TryValidateModel(cmdToPatch))
            {
                return ValidationProblem(ModelState); // model not validated 400 response;
            }
            _map.Map(cmdToPatch, getCommandModelObj);    // applying the updated change back to model so dbcontext can track changes and
                                                        // therefore save it so we must map back to a command model obj 

            _repo.UpdateCommand(getCommandModelObj); // unimplemented repository command
            _repo.SaveChanges();
            return NoContent();
        }

        //DELETE /api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {   
            var getCommandModel = _repo.GetCommandById(id);
            if(getCommandModel==null)
            {
                return NotFound();
            }
            _repo.DeleteCommand(getCommandModel);
            _repo.SaveChanges();
            
            return NoContent();
        }

    }
}