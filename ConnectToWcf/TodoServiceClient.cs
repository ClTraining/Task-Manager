using System;
using System.Collections.Generic;
using System.ServiceModel;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary;
using TaskManagerServiceLibrary.Converters;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.ToDoList;
using Xunit;

namespace ConnectToWcf
{
    public class ToDoServiceClient : IClient
    {
        private readonly NetTcpBinding binding;
        private readonly string serviceAddress;

        public ToDoServiceClient(string address)
        {
            serviceAddress = address;
            binding = new NetTcpBinding();
        }

        public int AddTask(AddTaskArgs args)
        {
            return GetDataFromServer(t => t.AddTask(args));
        }

        public List<ClientTask> GetTasks(IListCommandArguments args)
        {
            return GetDataFromServer(s => s.GetTasks(args));
        }

        public void ExecuteCommand(IEditCommandArguments args)
        {
            UpdateDataOnServer(s => s.UpdateChanges(args));
        }

        private void UpdateDataOnServer(Action<ITaskManagerService> action)
        {
            GetDataFromServer<object>(s =>
                                          {
                                              action(s);
                                              return null;
                                          });
        }

        private T GetDataFromServer<T>(Func<ITaskManagerService, T> func)
        {
            var client = new ChannelFactory<ITaskManagerService>(binding, serviceAddress);
            client.Open();
            try
            {
                return func(client.CreateChannel());
            }
            catch (FaultException<ExceptionDetail> e)
            {
                if (e.Detail.Type.Contains("TaskNotFoundException"))
                    throw new TaskNotFoundException(int.Parse(e.Detail.Message));
                if (e.Detail.Type.Contains("CouldNotSetDateException"))
                    throw new CouldNotSetDateException(e.Message);
                throw new Exception(e.Message);
            }
            catch (EndpointNotFoundException e)
            {
                throw new ServerNotAvailableException();
            }
            finally
            {
                CloseClient(client);
            }
        }

        private static void CloseClient(ChannelFactory<ITaskManagerService> client)
        {
            try
            {
                client.Close();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }
        }
    }

    public class ExchangeClientTests
    {
        private const string address = "net.tcp://localhost:44445";

        private readonly IClient client = new ToDoServiceClient(address);
        private readonly IArgToCommandConverter comConverter = Substitute.For<IArgToCommandConverter>();
        private readonly ServiceHost host;
        private readonly ITodoList list = Substitute.For<ITodoList>();
        private readonly IRepository repo = Substitute.For<IRepository>();
        private readonly ITaskManagerService service;
        private readonly ISpecificationsConverter specConverter = Substitute.For<ISpecificationsConverter>();

        public ExchangeClientTests()
        {
            service = new TaskManagerService(list, comConverter, specConverter);
            host = new ServiceHost(service, new Uri(address));
            host.Open();
        }

        [Fact]
        public void should_add_task_to_service()
        {
            const string myname = "myName";
            var args = new AddTaskArgs {Name = myname};
            service.AddTask(Arg.Is<AddTaskArgs>(a => a.Name == myname)).Returns(1);
            var result = client.AddTask(args);
            Console.Out.WriteLine(result);
            host.Close();

            result.Should().Be(1);
        }

        [Fact(Skip = "")]
        public void should_get_tasks_from_server()
        {
            var task = new ClientTask {Id = 1};
            var tasks = new List<ClientTask> {task};
            var args = new ListAllTaskArgs();

            service.GetTasks(Arg.Is(args)).Returns(tasks);
            var resultList = client.GetTasks(args);
            host.Close();
            Console.Out.WriteLine(resultList.GetType());
        }
    }
}