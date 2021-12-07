using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public class MockCommandRepo : ICommanderRepo
    {
        public IEnumerable<Command> GetAllCommands()
        {
            var commands = new List<Command>{
               new Command { Id = 0, HowTo = "Boil an egg", Line = "Boil water", Platform = "Kettle and saucepan" },
               new Command { Id = 1, HowTo = "Cut Bread", Line = "Get a knife", Platform = "Knife & chopping board" },
               new Command { Id = 2, HowTo = "Make a cup of tea", Line = "Place teabag in cup", Platform = "Kettle and cup" }

           };
           return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command { Id = 0, HowTo = "Boil an egg", Line = "Boil water", Platform = "Kettle and saucepan" };
        }
    }

}