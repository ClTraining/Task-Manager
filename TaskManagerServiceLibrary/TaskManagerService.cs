using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;
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
        private readonly List<IQuerySpecification> list;

        public TaskManagerService(IRepository repository, List<IQuerySpecification> list)
        {
            this.repository = repository;
            this.list = list;
        }

        public int AddTask(AddTaskArgs task)
        {
            return repository.AddTask(task);
        }

        public List<ContractTask> GetTasks(IClientSpecification input)
        {
            var res = list.First(x => x.GetType().Name.Contains(input.GetType().Name));
            res.Initialise(input.Data);
            return repository.GetTasks(res);
        }

        public void Complete(CompleteTaskArgs args)
        {
            repository.Complete(args);
        }

        public void RenameTask(RenameTaskArgs args)
        {
            repository.RenameTask(args);
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            repository.SetTaskDueDate(args);
        }

        public void ClearTaskDueDate(ClearDateArgs args)
        {
            repository.ClearTaskDueDate(args);
        }
    }

    public class TaskManagerTests
    {
        private readonly List<IQuerySpecification> specs = new StandardKernel(new MyModule()).GetAll<IQuerySpecification>().ToList();
        readonly ITaskMapper mapper = new TaskMapper();
        private readonly IRepository repo;

        readonly ITaskManagerService service;

        public TaskManagerTests()
        {
            repo = new MemoRepository(mapper);
            service = new TaskManagerService(repo, specs);
        }

        [Fact]
        public void should_get_tasks()
        {
            var spec = new ListSingle { Data = 3 };
            var tasks = new[] { "task1", "task2", "task3", "task4", "task5", "task6", "task7", "task8", "task9" };

            var addTaskArgs = new AddTaskArgs { Name = "some task" };

            service.AddTask(addTaskArgs);

            tasks.ToList().ForEach(a => service.AddTask(new AddTaskArgs { Name = a }));

            var result = service.GetTasks(spec);
        }
        
        [Fact]
        public void should_send_clear_date_for_task()
        {
            var dateTime = DateTime.Now;
            var args = new ClearDateArgs {Id = 1};
            service.ClearTaskDueDate(args);
            repo.Received().ClearTaskDueDate(args);
        }
    }

    public class MyModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromAssemblyContaining<IQuerySpecification>()
                               .SelectAllClasses()
                               .InNamespaceOf<IQuerySpecification>()
                               .BindAllInterfaces()
                );
        }
    }
}