using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using TaskManagerServiceLibrary;

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

        public void RenameTask(RenameTaskArgs args)
        {
            UpdateDataOnServer(t => t.RenameTask(args));
        }

        public int AddTask(AddTaskArgs task)
        {
            return GetDataFromServer(t => t.AddTask(task));
        }

        public List<ContractTask> GetTaskById(int id)
        {
            return GetDataFromServer(s => new List<ContractTask> {s.GetTaskById(id)});
        }

        public List<ContractTask> GetAllTasks()
        {
            return GetDataFromServer(s => s.GetAllTasks());
        }

        public void MarkTaskAsCompleted(CompleteTaskArgs id)
        {
            UpdateDataOnServer(s => s.MarkTaskAsCompleted(id));
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