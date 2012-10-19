using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Repositories
{
    public interface IRepository
    {
        int AddTask(AddTaskArgs name);
        ServiceTask GetTaskById(int id);
        List<ServiceTask> GetAllTasks();
        void MarkTaskAsCompleted(CompleteTaskArgs id);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}