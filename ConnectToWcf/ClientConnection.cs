using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.Arguments.AddTask;
using EntitiesLibrary.Arguments.SetDate;
using Specifications.ClientSpecification;
using TaskManagerServiceLibrary;

namespace ConnectToWcf
{
    public class ClientConnection : IClientConnection
    {
        private readonly string serviceAddress;
        private readonly NetTcpBinding binding;

        public ClientConnection(string address)
        {
            serviceAddress = address;
            binding = new NetTcpBinding();
        }

//        public void RenameTask(RenameTaskArgs args)
//        {
//            UpdateDataOnServer(t => t.RenameTask(args));
//        }

        public int AddTask(AddTaskArgs task)
        {
            return GetDataFromServer(t => t.AddTask(task));
        }

        public List<ContractTask> GetTasks(DataPackage pack)
        {
            return GetDataFromServer(s => s.GetTasks(pack));
        }

//        public void RenameTask(RenameTaskArgs args)
//        {
//            return GetDataFromServer(s => s.GetAllTasks());
//        }

//        public void Complete(int input)
//        {
//            UpdateDataOnServer(s => s.Complete(input));
//        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            UpdateDataOnServer(s => s.SetTaskDueDate(args));
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
                throw new TaskNotFoundException(e.Detail.Message);
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
}