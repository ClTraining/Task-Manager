#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml.Serialization;
using EntitiesLibrary;
using FluentAssertions;
using TaskManagerHost.WCFServer;
using Xunit;

#endregion

namespace TaskManagerHost.DataBaseAccessLayer
{
    public class XmlRepository: IRepository
    {
        private readonly string fileName;
        private List<ServiceTask> taskList;
        private readonly XmlSerializer serializer;

        private List<ServiceTask> TaskList 
        {
            get
            {
                var fileStream = new FileStream(fileName, FileMode.Open);
                var list = (List<ServiceTask>) serializer.Deserialize(fileStream);
                fileStream.Close();
                return list;
            }
            set
            {
                var writer = new StreamWriter(fileName);
                serializer.Serialize(writer, value);
                writer.Close();
            }
        }
        
        public XmlRepository(string fileName)
        {
            this.fileName = fileName;

            serializer = new XmlSerializer(typeof(List<ServiceTask>));

            if (!File.Exists(fileName))
            {
                TaskList = new List<ServiceTask>();
            }

            taskList = TaskList;
        }

        public int AddTask(string name)
        {
            var task = new ServiceTask {Name = name, Id = GetNewId()};

            taskList.Add(task);
            TaskList = taskList;

            return task.Id;
        }

        public ServiceTask GetTaskById(int id)
        {
            var task = taskList.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                throw new TaskNotFoundException(id);
            }

            return task;
        }

        public List<ServiceTask> GetAllTasks()
        {
            return TaskList.ToList();
        }

        public void MarkCompleted(int id)
        {
            var taskToEdit = GetTaskById(id);

            taskToEdit.IsCompleted = true;

            TaskList = taskList;
        }

        public void DeleteAllTasks()
        {
            taskList = new List<ServiceTask>();
            TaskList = taskList;
        }

        private int GetNewId()
        {
            var newId = 0;

            if (TaskList.Any())
            {
                newId = TaskList.Max(x => x.Id);
            }

            return ++newId;
        }
    }

    public class XmlRepositoryTests
    {
        private readonly XmlRepository repository = new XmlRepository("test");
        private readonly List<string> taskNames = new List<string> { "test task", "another task", "my task" };

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            repository.DeleteAllTasks();
            var tTask = repository.AddTask(taskNames[0]);
            tTask.Should().Be(1);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            repository.DeleteAllTasks();
            Action act = () => repository.GetTaskById(1);
            act.ShouldThrow<TaskNotFoundException>();
        }

        [Fact]
        public void should_get_task_by_id()
        {
            repository.DeleteAllTasks();
            var addedTasks = taskNames.Select(repository.AddTask);
            var receivedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            foreach (var task in receivedTasks)
            {
                task.Name.Should().Be(taskNames.ToArray()[receivedTasks.ToList().IndexOf(task)]);
            }
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            repository.DeleteAllTasks();
            Action act = () =>repository.MarkCompleted(10);
            act.ShouldThrow<TaskNotFoundException>();
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            repository.DeleteAllTasks();
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
            repository.DeleteAllTasks();
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
        }

        [Fact]
        public void should_get_all_tasks()
        {
            repository.DeleteAllTasks();
            taskNames.Select(repository.AddTask).ToList();
            var taskList = repository.GetAllTasks().ToList();
            foreach (var task in taskList)
            {
                task.Name.Should().Be(taskNames.ToArray()[taskList.IndexOf(task)]);
            }
        }
    }

}
