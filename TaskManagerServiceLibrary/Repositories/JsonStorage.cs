using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class JsonStorage : IRepository
    {

        private readonly List<ServiceTask> cacheStorage;
        private int currentId;
        private const string FileName = "save.txt";

        public JsonStorage()
        {
            var jsonString = GetJsonString();
            cacheStorage = DeserializeToListOfTasks(jsonString);
            currentId = cacheStorage.Max(t => t.Id);
        }
        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask { Name = args.Name, DueDate = args.DueDate == null ? default(DateTime) : args.DueDate.Value, Id = GetNewId() };
            AddToCache(serviceTask);
            SynchronizeCacheAndLocal();
            return serviceTask.Id;
        }

        public ServiceTask Select(int id)
        {
            return cacheStorage[id - 1];
        }

        public void UpdateChanges(ServiceTask task)
        {
            UpdateCacheStorage(task);
            SynchronizeCacheAndLocal();
        }

        private void UpdateCacheStorage(ServiceTask task)
        {
            cacheStorage[task.Id - 1] = task;
        }

        public List<ServiceTask> GetTasks(IServiceSpecification spec)
        {
            var resList = cacheStorage
                .Where(spec.IsSatisfied)
                .ToList();

            return resList;
        }

        private void AddToCache(ServiceTask serviceTask)
        {
            cacheStorage.Add(serviceTask);
        }

        private string GetJsonString()
        {
            string text;
            try
            {
                text = File.ReadAllText(FileName);
            }
            catch (FileNotFoundException)
            {
                text = "[]";
            }
            DeserializeToListOfTasks(text);
            return text;
        }

        private List<ServiceTask> DeserializeToListOfTasks(string text)
        {
            return JsonConvert.DeserializeObject<List<ServiceTask>>(text);
        }

        private void SynchronizeCacheAndLocal()
        {
            var stream = new FileStream(FileName, FileMode.Create);
            var writer = new StreamWriter(stream);
            var serializeObject = JsonConvert.SerializeObject(cacheStorage);
            writer.Write(serializeObject);
            writer.Close();
            stream.Close();
        }

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class JsonStorageTests
    {
        private readonly JsonStorage storage;

        public JsonStorageTests()
        {
            storage = new JsonStorage();
        }

        [Fact]
        public void should_write_task_to_save_file()
        {
            var args = new AddTaskArgs { DueDate = DateTime.Today, Name = "sasha" };
            storage.AddTask(args);
            var text = File.ReadAllText("save.txt");
            text.Should().NotBeEmpty();
        }

        [Fact]
        public void should_get_tasks_by_specification()
        {
            IServiceSpecification specification = new ListAllServiceSpecification();
            var tasks = storage.GetTasks(specification);

            tasks.Count.Should().BeGreaterOrEqualTo(0);
        }
    }
}