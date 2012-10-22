using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Specifications.ClientSpecification;
using Specifications.Mappers;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;
using System.Linq;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IRepository repository;
        private readonly IToDoList taskList;
        private readonly List<IServiceSpecification> list;

        public TaskManagerService(IRepository repository, List<IServiceSpecification> list, IToDoList taskList)
        {
            this.repository = repository;
            this.list = list;
            this.taskList = taskList;
        }

        public int AddTask(string task)
        {
            return taskList.AddTask(task);
        }

        public List<ContractTask> GetTasks(IClientSpecification specification)
        {
            var res = list.First(x => x.GetType().Name.Contains(specification.GetType().Name));

            return repository.GetTasks(res);
        }
    }

    public class TaskManagerServiceTests
    {
        private readonly List<IServiceSpecification> specs;

        public TaskManagerServiceTests(List<IServiceSpecification> specs)
        {
            this.specs = specs;
        }

        [Fact]
        public void should_get_tasks_from_repository()
        {
            const int id = 3;
            var cSpec = new ListSingle(id);

            var repo = new MemoRepository();
            var todolist = new ToDoList(repo);

            var service = new TaskManagerService(repo, specs, todolist);

            var tasks = new[] { "task1", "task2", "task3", "task4", "task5", "task6", "task7", "task8", "task9" };

            tasks.ToList().ForEach(x => service.AddTask(x));

            var result = service.GetTasks(cSpec);

            foreach (var task in result)
            {
                Console.Out.WriteLine(task.Id + " " + task.Name);
            }
        }
    }
}