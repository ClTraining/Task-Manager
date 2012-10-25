using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;
using TaskManagerServiceLibrary;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;

namespace ConnectToWcf
{
    public class ClientConnection : IClientConnection
    {
        private readonly NetTcpBinding binding;
        private readonly string serviceAddress;

        public ClientConnection(string address)
        {
            serviceAddress = address;
            binding = new NetTcpBinding();
        }

        public int AddTask(AddTaskArgs task)
        {
            return GetDataFromServer(t => t.AddTask(task));
        }

        public void Complete(CompleteTaskArgs args)
        {
            UpdateDataOnServer(s => s.Complete(args));
        }

        public void RenameTask(RenameTaskArgs args)
        {
            UpdateDataOnServer(t => t.RenameTask(args));
        }

        public List<ContractTask> GetTasks(IClientSpecification data)
        {
            return GetDataFromServer(s => s.GetTasks(data));
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            UpdateDataOnServer(s => s.SetTaskDueDate(args));
        }

        public void ClearTaskDueDate(ClearDateArgs args)
        {
            UpdateDataOnServer(s => s.ClearTaskDueDate(args));
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
                throw new TaskNotFoundException();
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

    public class ClientConnectionTests
    {
        private const string address = "net.tcp://localhost:44440";

        private readonly IRepository repo = Substitute.For<IRepository>();
        private readonly IClientConnection client = new ClientConnection(address);
        private readonly ITaskManagerService service;
        private readonly ServiceHost host;

        public ClientConnectionTests()
        {
            service = new TaskManagerService(repo);
            host = new ServiceHost(service, new Uri(address));
            host.Open();
        }

        [Fact]
        public void should_add_task_to_service()
        {
            const string myname = "myName";
            var args = new AddTaskArgs { Name = myname };
            repo.AddTask(Arg.Is<AddTaskArgs>(a => a.Name == myname)).Returns(1);
            
            var result = client.AddTask(args);
            host.Close();

            result.Should().Be(1);
        }

        [Fact]
        public void should_mark_task_as_complited()
        {
            var args = new CompleteTaskArgs { Id = 1 };
            
            client.Complete(args);

            repo.Received().Complete(Arg.Is<CompleteTaskArgs>(a => a.Id == 1));
            host.Close();
        }

        [Fact]
        public void should_rename_task_by_id()
        {
            var args = new RenameTaskArgs { Id = 1 };

            client.RenameTask(args);

            repo.Received().RenameTask(Arg.Is<RenameTaskArgs>(a => a.Id == 1));
            host.Close();
        }

        [Fact(Skip = "")]
        public void should_get_tasks_from_server()
        {
            var expected = new List<ContractTask> {new ContractTask {Id = 1}};
            var cSpec = Substitute.For<IClientSpecification>();
            //cSpec.Data.Returns(1);
            var qSpec = Substitute.For<IQuerySpecification>();

            repo.GetTasks(Arg.Is<IQuerySpecification>(s => s.IsSatisfied(new ServiceTask{Id = 1}))).Returns(expected);

            var result = client.GetTasks(cSpec);

            foreach (var task in result)
            {
                Console.WriteLine(task);
            }

            host.Close();
        }

        [Fact]
        public void should_set_date_to_task()
        {
            var args = new SetDateArgs{Id = 1, DueDate = DateTime.Today};

            client.SetTaskDueDate(args);

            repo.Received().SetTaskDueDate(Arg.Is<SetDateArgs>(a => a.Id == 1));
            host.Close();
        }

        [Fact]
        public void should_clear_task_by_date()
        {
            var args = new ClearDateArgs{Id = 1};

            client.ClearTaskDueDate(args);

            repo.Received().ClearTaskDueDate(Arg.Is<ClearDateArgs>(a => a.Id == 1));
            host.Close();
        }
    }
}