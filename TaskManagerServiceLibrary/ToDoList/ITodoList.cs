using System;
using System.Collections.Generic;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary.ToDoList
{
    public interface ITodoList
    {
        int AddTask(AddTaskArgs args);
        List<ClientTask> GetTasks(IServiceSpecification serviceSpecification);
        void ClearDate(int id);
        void CompleteTask(int id);
        void RenameTask(int id, string newName);
        void SetTaskDate(int id, DateTime dueDate);
        ServiceTask SelectTaskById(int id);
    }
}