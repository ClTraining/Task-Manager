using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Specifications.QuerySpecifications;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class MemoRepository : IRepository
    {
        private readonly ITaskMapper mapper;
        public readonly List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        public MemoRepository(ITaskMapper mapper)
        {
            this.mapper = mapper;
        }
        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask { Name = args.Name, DueDate = args.DueDate, Id = GetNewId() };

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public List<ContractTask> GetTasks(IQuerySpecification spec)
        {
            return taskList
                .Where(spec.IsSatisfied)
                .Select(mapper.ConvertToContract)
                .ToList();
        }

        public void Complete(CompleteTaskArgs args)
        {
            taskList.First(x => x.Id == args.Id).IsCompleted = true;
        }

        public void RenameTask(RenameTaskArgs args)
        {
            taskList.First(x => x.Id == args.Id).Name = args.Name;
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            taskList.First(x => x.Id == args.Id).DueDate = args.DueDate;
        }

        public void ClearTaskDueDate(ClearDateArgs args)
        {
            taskList.First(x => x.Id == args.Id).DueDate = default(DateTime);
        }

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class MemoRepositoryTests
    {
        private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
        private readonly IQuerySpecification spec = Substitute.For<IQuerySpecification>();
        readonly MemoRepository repo;
        readonly ServiceTask sTask = new ServiceTask{Id = 1};

        public MemoRepositoryTests()
        {
            repo = new MemoRepository(mapper);
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

        [Fact]
        public void should_mark_task_as_completed()
        {
            repo.Complete(new CompleteTaskArgs{Id = 1});
            repo.taskList[0].IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void should_rename_task_by_id()
        {
            repo.RenameTask(new RenameTaskArgs{Id = 1, Name = "task1"});
            repo.taskList[0].Name.Should().Be("task1");
        }

        [Fact]
        public void should_set_date_to_task_by_id()
        {
            repo.SetTaskDueDate(new SetDateArgs{Id = 1, DueDate = DateTime.Today});
            repo.taskList[0].DueDate.Should().Be(DateTime.Today);
        }

        [Fact]
        public void should_clear_date_by_id()
        {
            repo.ClearTaskDueDate(new ClearDateArgs{Id = 1});
            repo.taskList[0].DueDate.Should().Be(default(DateTime));
        }
    }
}