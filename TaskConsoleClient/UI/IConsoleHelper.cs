using EntitiesLibrary;

namespace TaskConsoleClient.UI
{
    public interface IConsoleHelper
    {
        void Parse(string text);
        void View(ContractTask task);
    }
}