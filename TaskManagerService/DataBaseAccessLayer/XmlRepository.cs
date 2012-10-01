#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

#endregion

namespace TaskManagerHost.DataBaseAccessLayer
{
    class XmlRepository: IRepository
    {
        private readonly string fileName;
        private List<ServiceTask> TaskList {
            get
            {
                var fileStream = new FileStream(fileName, FileMode.Open);
                var retu = (List<ServiceTask>) serializer.Deserialize(fileStream);
                fileStream.Close();
                return retu;
            }
            set
            {
                var myWriter = new StreamWriter(fileName);
                serializer.Serialize(myWriter, value);
                myWriter.Close();
            }
        }
        private List<ServiceTask> taskList;
        private readonly XmlSerializer serializer;
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
            var task = TaskList.FirstOrDefault(t => t.Id == id);

            return task;
        }

        public List<ServiceTask> GetAllTasks()
        {
            return TaskList.ToList();
        }

        public bool MarkCompleted(int id)
        {
            var result = true;

            var taskToEdit = taskList.FirstOrDefault(t => t.Id == id);

            if (taskToEdit == null)
            {
                result = false;
            }
            else
            {
                taskToEdit.IsCompleted = true;
            }

            TaskList = taskList;

            return result;
        }

        public bool DeleteAllTasks()
        {
            taskList = new List<ServiceTask>();
            TaskList = taskList;
            return true;
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
        //private readonly IRepository repository = new XmlRepository("test");
        private readonly List<string> taskNames = new List<string> { "test task", "another task", "my task" };

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var repository = new XmlRepository("test1");
            repository.DeleteAllTasks();
            var tTask = repository.AddTask(taskNames[0]);
            tTask.Should().Be(1);
            File.Delete("test1");
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            var repository = new XmlRepository("test2");
            repository.DeleteAllTasks();
            var task = repository.GetTaskById(1);
            task.Should().BeNull();
            File.Delete("test2");
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var repository = new XmlRepository("test3");
            repository.DeleteAllTasks();
            var addedTasks = taskNames.Select(repository.AddTask);
            var getedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            foreach (var task in getedTasks)
            {
                task.Name.Should().Be(taskNames.ToArray()[getedTasks.ToList().IndexOf(task)]);
            }
            File.Delete("test3");
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var repository = new XmlRepository("test4");
            var result = repository.MarkCompleted(10);
            result.Should().Be(false);
            File.Delete("test4");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var repository = new XmlRepository("test5");
            var addedTasks = taskNames.Select(repository.AddTask).ToList();
            var compl = addedTasks.Select(repository.MarkCompleted).ToList();
            var getedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            foreach (var task in getedTasks)
            {
                task.IsCompleted.Should().Be(true);
            }
            File.Delete("test5");
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var repository = new XmlRepository("test6");
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
            var addedTasks = taskNames.Select(repository.AddTask).ToList();
            taskList = repository.GetAllTasks().ToList();
            foreach (var task in taskList)
            {
                task.Name.Should().Be(taskNames.ToArray()[taskList.IndexOf(task)]);
            }
            File.Delete("test6");
        }
    }

}
