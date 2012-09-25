using EntitiesLibrary;

namespace TaskConsoleClient.UI
{
    public interface IConsoleHelper
    {
        void View(ContractTask task);

        void Parse(string text);
    }
}
