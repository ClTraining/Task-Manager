using System.Collections.Generic;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface IToDoList
    {
        int AddTask(AddTaskArgs name);
        void MarkTaskAsCompleted(CompleteTaskArgs id);
        void RenameTask(RenameTaskArgs args);
        void SetTaskDueDate(SetDateArgs args);
    }
}