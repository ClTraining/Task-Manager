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
using NSubstitute;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class JsonStorage : IRepository
    {
        private readonly FileOperationsWrapper saver;
        private readonly List<ServiceTask> cacheStorage;
        private int currentId;

        public JsonStorage(FileOperationsWrapper saver)
        {
            this.saver = saver;
            var jsonString = GetJsonString();
            cacheStorage = DeserializeToListOfTasks(jsonString);
            currentId = cacheStorage.Any() ? cacheStorage.Max(t => t.Id) : 0;
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
            var task = cacheStorage.FirstOrDefault(t => t.Id == id);
            if (task == null) throw new TaskNotFoundException(id);
            return task;
        }

        public void UpdateChanges(ServiceTask task)
        {
            UpdateCacheStorage(task);
            SynchronizeCacheAndLocal();
        }

        private void UpdateCacheStorage(ServiceTask task)
        {
            cacheStorage[task.Id - 1] = task;
            SynchronizeCacheAndLocal();
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
            var info = saver.GetInfo();
            return info.Any() ? info : "[]";
        }

        private List<ServiceTask> DeserializeToListOfTasks(string text)
        {
            return JsonConvert.DeserializeObject<List<ServiceTask>>(text);
        }

        private void SynchronizeCacheAndLocal()
        {
            var serializeObject = JsonConvert.SerializeObject(cacheStorage);
            saver.SaveToFile(serializeObject);
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
        private readonly FileOperationsWrapper saver = Substitute.For<FileOperationsWrapper>();

        public JsonStorageTests()
        {
            storage = new JsonStorage(saver);
        }

        [Fact]
        public void should_write_task_to_save_file()
        {
            storage.AddTask(new AddTaskArgs());

            saver.ReceivedWithAnyArgs().SaveToFile("bla-bla");
        }

        [Fact]
        public void should_get_tasks_by_specification()
        {
            var spec = Substitute.For<IServiceSpecification>();
            storage.GetTasks(spec);

            saver.ReceivedWithAnyArgs().GetInfo();
        }

        [Fact]
        public void should_update_storage()
        {
            storage.AddTask(new AddTaskArgs { DueDate = DateTime.Today, Name = "Misha" });
            var task = new ServiceTask { Id = 1, DueDate = DateTime.Today, IsCompleted = false, Name = "Sasha" };

            storage.UpdateChanges(task);

            saver.ReceivedWithAnyArgs().SaveToFile("bla-bla");
        }

        [Fact]
        public void should_select_task_with_given_ID()
        {
            storage.AddTask(new AddTaskArgs { DueDate = DateTime.Today, Name = "Sasha" });
            storage.AddTask(new AddTaskArgs { DueDate = DateTime.Today, Name = "Misha" });
            storage.AddTask(new AddTaskArgs { DueDate = DateTime.Today, Name = "Pasha" });
            var serviceTask = storage.Select(1);
            serviceTask.Id.Should().Be(1);
        }
    }
}