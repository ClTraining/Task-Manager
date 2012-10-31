using System.Collections.Generic;
using System.Linq;
using CommandQueryLibrary.ServiceSpecifications;
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
        private readonly IArgToCommandConverter converter;

        public TodoList(IRepository repo, ITaskMapper mapper, IArgToCommandConverter converter)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.converter = converter;
        }

        public void UpdateChanges(IEditCommandArguments args)
        {
            converter.GetServiceCommand(args).ExecuteCommand(repo);
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
