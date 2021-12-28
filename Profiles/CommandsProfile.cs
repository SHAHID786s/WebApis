using AutoMapper; // for inherting from Profile class
using Commander.Dtos;
using Commander.Models;
namespace Commander.Profiles{
    public class CommandsProfile : Profile { 
        public CommandsProfile()
        {   
            //Source->target
            //to map we must transfer model to the map profile and then transfer the mapped object back to original otherwise db will not save it
             CreateMap<Command,CommandReadDto>();// map intialisation CreateMap<mapping source, mapping destination>(); 
             CreateMap<CommandCreateDto,Command>();
             CreateMap<CommandUpdateDto,Command>();
             CreateMap<Command,CommandUpdateDto>();// for patch request
             
        }
    }
}