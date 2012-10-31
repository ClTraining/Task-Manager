using System;
using System.Collections.Generic;
using System.Linq;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using TaskManagerServiceLibrary.Commands;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;

namespace TaskManagerServiceLibrary
{
    public class TodoList : ITodoList
    {
        private readonly IRepository repo;
        private readonly ITaskMapper mapper;
        private readonly List<IServiceCommand<IEditCommandArguments>> commands;

        public TodoList(IRepository repo, ITaskMapper mapper, List<IServiceCommand<IEditCommandArguments>> commands)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.commands = commands;
        }

        public void UpdateChanges(IEditCommandArguments args)
        {
            commands.First(x=>x.GetType().Name.Contains(args.GetType().Name.Split('T')[0])).ExecuteCommand(args);
        }

        public int AddTask(AddTaskArgs args)
        {
            return repo.AddTask(args);
        }

        public List<ClientTask> GetTasks(IServiceSpecification serviceSpecification)
        {
            return repo.GetTasks(serviceSpecification).Select(mapper.ConvertToContract).ToList();
        }
    }
}
