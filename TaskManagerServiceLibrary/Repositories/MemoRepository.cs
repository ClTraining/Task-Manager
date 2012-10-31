using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class MemoRepository : IRepository
    {
        public readonly List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask { Name = args.Name, DueDate = args.DueDate == null ? default(DateTime):args.DueDate.Value, Id = GetNewId() };

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public List<ServiceTask> GetTasks(IServiceSpecification spec)
        {
            return taskList.Where(spec.IsSatisfied).ToList();
        }

        public void UpdateChanges(IEditCommandArguments args)
        {
        }

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class MemoRepositoryTests
    {
        private readonly IServiceSpecification spec = Substitute.For<IServiceSpecification>();
        private readonly MemoRepository repo = new MemoRepository();
        readonly ServiceTask sTask = new ServiceTask{Id = 1};

        public MemoRepositoryTests()
        {
            repo.taskList.Add(sTask);
        }

        [Fact]
        public void should_add_task_to_repo()
        {
            var task = new AddTaskArgs { DueDate = DateTime.Now, Name = "task1" };
            
            var result = repo.AddTask(task);
            
            result.Should().Be(1);
        }

        [Fact]
        public void should_get_all_tasks_from_repo()
        {
            spec.IsSatisfied(sTask).Returns(true);
            
            var res = repo.GetTasks(spec);
            
            res.Count.Should().Be(1);
        }
    }
}