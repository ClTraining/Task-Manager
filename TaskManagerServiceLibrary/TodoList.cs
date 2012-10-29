using System;
using System.Collections.Generic;
using System.Linq;
using CQRS.Commands;
using CQRS.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;

namespace TaskManagerServiceLibrary
{
    public class TodoList : ITodoList
    {
        private readonly IRepository repo;
        private readonly ITaskMapper mapper;
        private readonly List<IServiceCommand> commands;

        public TodoList(IRepository repo, ITaskMapper mapper, List<IServiceCommand> commands)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.commands = commands;
        }

        public void UpdateChanges(ICommandArguments args)
        {
            var tasks = repo.GetTasks(new ListSingleServiceSpecification {Id = args.Id});
            commands.First(
                c => c.GetType().Name.Contains(args.GetType().Name.Split(new[] {"Task"}, StringSplitOptions.None)[0])).
                Update(args, tasks[0]);
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
