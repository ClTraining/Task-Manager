#region Using

using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

#endregion


namespace TaskManagerHost.DataBaseAccessLayer
{
    public class MemoRepository : IRepository
    {
        readonly List<ServiceTask> taskList = new List<ServiceTask>();

        public int AddTask(string name)
        {
            taskList.Add(new ServiceTask {Name = name, Id = GetNewId()});

            return taskList.Select(x => x.Id).Last();
        }

        public ServiceTask GetTaskById(int id)
        {
            return taskList.FirstOrDefault(t => t.Id == id);
        }

        public List<ServiceTask> GetAllTasks()
        {
            return taskList.ToList();
        }

        public bool MarkCompleted(int id)
        {
            var result = true;

            var taskToEdit = taskList.FirstOrDefault(t => t.Id == id);

            if (taskToEdit == null)
                result = false;
            else
                taskToEdit.IsCompleted = true;

            return result;
        }

        private int GetNewId()
        {
           var newId = 0;

            if (taskList.Any())
            {
                newId = taskList.Max(x => x.Id);
            }

            return ++newId;
        }
    }

    public class MemoRepositoryTests
    {
        private readonly IRepository repository = new MemoRepository();
        private readonly List<string> taskNames = new List<string> { "test task", "another task", "my task" }; 

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var tTask = repository.AddTask(taskNames[0]);
            tTask.Should().Be(1);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            var task = repository.GetTaskById(1);
            task.Should().BeNull();
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var addedTasks = taskNames.Select(repository.AddTask);
            var gettedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            gettedTasks.Select(x=> x.Name.Should().Be(taskNames.ToArray()[gettedTasks.ToList().IndexOf(x)]));
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var result =  repository.MarkCompleted(10);
            result.Should().Be(false);
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var addedTasks = taskNames.Select(repository.AddTask).ToList();
            var compl = addedTasks.Select(repository.MarkCompleted).ToList();
            var getedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            foreach (var task in getedTasks)
            {
                task.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
            var addedTasks = taskNames.Select(repository.AddTask).ToList();
            taskList = repository.GetAllTasks().ToList();
            foreach (var task in taskList)
            {
                task.Name.Should().Be(taskNames.ToArray()[taskList.IndexOf(task)]);
            }
        }

    }
}