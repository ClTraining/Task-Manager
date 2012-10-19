using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class MemoRepository : IRepository
    {
        private readonly List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        #region IRepository Members

        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask {Name = args.Name, DueDate = args.DueDate, Id = GetNewId()};

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public ServiceTask GetTaskById(int id)
        {
            int index = id - 1;
            if (index < 0 || index >= taskList.Count)
                throw new TaskNotFoundException(id.ToString());

            return taskList[index];
        }

        public List<ServiceTask> GetAllTasks()
        {
            return taskList.ToList();
        }

        public void MarkTaskAsCompleted(CompleteTaskArgs args)
        {
            GetTaskById(args.Id).IsCompleted = true;
        }

        public void RenameTask(RenameTaskArgs args)
        {
            GetTaskById(args.Id).Name = args.Name;
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            GetTaskById(args.Id).DueDate = args.DueDate;
        }

        #endregion

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class MemoRepositoryTests
    {
        private readonly MemoRepository repository = new MemoRepository();
        private readonly List<string> taskNames = new List<string> {"test task", "another task", "my task"};

        [Fact]
        public void should_throw_exception_if_index_not_found()
        {
            Action action = () => repository.GetTaskById(4);
            action.ShouldThrow<TaskNotFoundException>().WithMessage("4");
        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            int tTask = repository.AddTask(new AddTaskArgs {Name = taskNames[0]});
            tTask.Should().Be(1);
        }

        [Fact]
        public void should_get_task_by_id()
        {
            List<int> addedTasks =
                taskNames.Select(taskName => repository.AddTask(new AddTaskArgs {Name = taskName})).ToList();
            List<ServiceTask> receivedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            receivedTasks.Select(x => x.Name.Should().Be(taskNames.ToArray()[receivedTasks.ToList().IndexOf(x)]));
        }

        [Fact]
        public void should_mark_task_by_id()
        {
            int taskId = repository.AddTask(new AddTaskArgs {Name = "tt"});
            repository.MarkTaskAsCompleted(new CompleteTaskArgs {Id = taskId});
            repository.GetTaskById(taskId).IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void should_return_empty_list()
        {
            List<ServiceTask> result = repository.GetAllTasks();
            result.ShouldBeEquivalentTo(new List<ServiceTask>());
        }

        [Fact]
        public void should_get_all_tasks()
        {
            taskNames.Select(taskName => repository.AddTask(new AddTaskArgs {Name = taskName})).ToList();
            List<ServiceTask> taskList = repository.GetAllTasks().ToList();
            foreach (ServiceTask task in taskList)
            {
                task.Name.Should().Be(taskNames.ToArray()[taskList.IndexOf(task)]);
            }
        }

        [Fact]
        public void should_rename_task()
        {
            int taskId = repository.AddTask(new AddTaskArgs {Name = "tt"});
            repository.RenameTask(new RenameTaskArgs {Id = taskId, Name = "New name"});
            repository.GetTaskById(taskId).Name.Should().Be("New name");
        }

        [Fact]
        public void shoud_set_date_for_task()
        {
            int taskId = repository.AddTask(new AddTaskArgs { Name = "tt" });
            var dateTime = DateTime.Now;
            repository.SetTaskDueDate(new SetDateArgs { Id = taskId, DueDate = dateTime });
            repository.GetTaskById(taskId).DueDate.Should().Be(dateTime);
        }
    }
}