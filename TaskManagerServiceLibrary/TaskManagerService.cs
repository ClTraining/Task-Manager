using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.AddTask;
using EntitiesLibrary.Arguments.CompleteTask;
using EntitiesLibrary.Arguments.RenameTask;
using EntitiesLibrary.Arguments.SetDate;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Specifications.ClientSpecification;
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

        public int AddTask(AddTaskArgs task)
        {
            return taskList.AddTask(task);
        }

        public List<ContractTask> GetTasks(DataPackage pack)
        {
            var res = list.First(x => x.GetType().Name.Contains(pack.Spec.GetType().Name));

            res.Data = pack.Spec.Id;

            Console.WriteLine(res.Data);

            return repository.GetTasks(res);
        }

        public void Complete(CompleteTaskArgs args)
        {
            taskList.Complete(args);
        }

        public void RenameTask(RenameTaskArgs args)
        {
            taskList.RenameTask(args);
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            taskList.SetTaskDueDate(args);
        }
    }

    public class TaskManagerTests
    {
        private readonly List<IServiceSpecification> specs = new StandardKernel(new MyModule()).GetAll<IServiceSpecification>().ToList();
        readonly IRepository repo = new MemoRepository(new TaskMapper());
        readonly IToDoList todolist;

        readonly ITaskManagerService service;

        public TaskManagerTests()
        {
            todolist = new ToDoList(repo);
            service = new TaskManagerService(repo, specs, todolist);
        }

        [Fact]
        public void should_get_tasks()
        {
            var pack = new DataPackage {Spec = new ListSingle {Id = 4}};

            var tasks = new[] {"task1", "task2", "task3", "task4", "task5", "task6", "task7", "task8", "task9"};

            var addTaskArgs = new AddTaskArgs {Name = "some task"};

            service.AddTask(addTaskArgs);

            tasks.ToList().ForEach(a => service.AddTask(new AddTaskArgs{Name = a}));

            var result = service.GetTasks(pack);

            foreach (var task in result)
            {
            }
        }
    }

    public class MyModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromAssemblyContaining<IServiceSpecification>()
                               .SelectAllClasses()
                               .InNamespaceOf<IServiceSpecification>()
                               .BindAllInterfaces()
                );
        }
    }
}
