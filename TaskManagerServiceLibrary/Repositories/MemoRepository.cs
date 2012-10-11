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
        readonly List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        public int AddTask(string name)
        {
            var serviceTask = new ServiceTask { Name = name, Id = GetNewId() };

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public ServiceTask GetTaskById(int id)
        {
            var index = id - 1;
            if (index < 0 || index >= taskList.Count)
                throw new TaskNotFoundException(id);

            return taskList[index];
        }

        public List<ServiceTask> GetAllTasks()
        {
            return taskList.ToList();
        }

        public void Complete(int id)
        {
            Console.Out.WriteLine("id = {0}", id);
            GetTaskById(id).IsCompleted = true;
        }

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class MemoRepositoryTests
    {
        private readonly List<string> taskNames = new List<string> { "test task", "another task", "my task" };
        readonly MemoRepository repository = new MemoRepository();

        [Fact]
        public void should_throw_exception_if_index_not_found()
        {
            Action action = () => repository.GetTaskById(4);
            action.ShouldThrow<TaskNotFoundException>();
        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var tTask = repository.AddTask(taskNames[0]);
            tTask.Should().Be(1);
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var addedTasks = taskNames.Select(repository.AddTask);
            var receivedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            receivedTasks.Select(x => x.Name.Should().Be(taskNames.ToArray()[receivedTasks.ToList().IndexOf(x)]));
        }

        [Fact]
        public void should_mark_task_by_id()
        {
            var taskId = repository.AddTask("tt");
            repository.Complete(taskId);
            repository.GetTaskById(taskId).IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void should_return_empty_list()
        {
            var result = repository.GetAllTasks();
            result.ShouldBeEquivalentTo(new List<ServiceTask>());
        }

        [Fact]
        public void should_get_all_tasks()
        {
            taskNames.Select(repository.AddTask).ToList();
            var taskList = repository.GetAllTasks().ToList();
            foreach (var task in taskList)
            {
                task.Name.Should().Be(taskNames.ToArray()[taskList.IndexOf(task)]);
            }
        }

    }
}