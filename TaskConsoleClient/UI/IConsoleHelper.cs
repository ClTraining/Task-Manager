#region Using

using TaskConsoleClient.Entities;
using TaskConsoleClient.Manager;

#endregion


namespace TaskConsoleClient.UI
{
    public interface IConsoleHelper
    {
        void View(TaskContract task);

        void Parse(string text);
    }
}
