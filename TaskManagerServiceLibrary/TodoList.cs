using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;

namespace TaskManagerServiceLibrary
{
    public class TodoList : ITodoList
    {
        private readonly IRepository repo;
        private readonly ITaskMapper mapper;

        public TodoList(IRepository repo, ITaskMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public void UpdateChanges(ICommandArguments args)
        {
            var tasks = repo.GetTasks(new ListSingleServiceSpecification {Id = args.Id});

            if (args is ClearDateTaskArgs)
                tasks[0].DueDate = default(DateTime);
            else if (args is CompleteTaskArgs)
                tasks[0].IsCompleted = true;
            else if (args is RenameTaskArgs)
                tasks[0].Name = (args as RenameTaskArgs).Name;
            else if (args is SetDateTaskArgs)
                tasks[0].DueDate = (args as SetDateTaskArgs).DueDate;
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
