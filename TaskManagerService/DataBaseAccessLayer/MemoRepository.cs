#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using FluentAssertions;
using TaskManagerHost.WCFServer;
using Xunit;

#endregion


namespace TaskManagerHost.DataBaseAccessLayer
{
    public class MemoRepository : IRepository
    {
        List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId = 0;

        public int AddTask(string name)
        {
            var serviceTask = new ServiceTask {Name = name, Id = GetNewId()};

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public ServiceTask GetTaskById(int id)
        {
            return taskList.FirstOrDefault(t => t.Id == id);
        }

        public List<ServiceTask> GetAllTasks()
        {
            return taskList.ToList();
        }

        public void MarkCompleted(int id)
        {
            var taskToEdit = GetTaskById(id);

            if (taskToEdit == null)
            {
                throw new TaskNotFoundException("Task with id does not exist.", id);
            }
            
            taskToEdit.IsCompleted = true;
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

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var repository = new MemoRepository();
            var tTask = repository.AddTask(taskNames[0]);
            tTask.Should().Be(1);
        }

        [Fact]
        public void should_return_null_when_task_was_not_found()
        {
            var repository = new MemoRepository();
            var task = repository.GetTaskById(1);
            task.Should().BeNull();
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var repository = new MemoRepository();
            var addedTasks = taskNames.Select(repository.AddTask);
            var receivedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            receivedTasks.Select(x => x.Name.Should().Be(taskNames.ToArray()[receivedTasks.ToList().IndexOf(x)]));
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var repository = new MemoRepository();
            Action act = () => repository.MarkCompleted(10);
            act.ShouldThrow<TaskNotFoundException>().WithMessage("Task with id does not exist.");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var repository = new MemoRepository();
            var addedTaskIds = taskNames.Select(repository.AddTask).ToList();
            foreach (var addedTaskId in addedTaskIds)
            {
                repository.MarkCompleted(addedTaskId);
            }
            var receivedTasks = addedTaskIds.Select(repository.GetTaskById).ToList();
            foreach (var task in receivedTasks)
            {
                task.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void should_return_empty_list()
        {
            var repository = new MemoRepository();
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());            
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var repository = new MemoRepository();
            taskNames.Select(repository.AddTask).ToList();
            var taskList = repository.GetAllTasks().ToList();
            foreach (var task in taskList)
            {
                task.Name.Should().Be(taskNames.ToArray()[taskList.IndexOf(task)]);
            }
        }

    }
}