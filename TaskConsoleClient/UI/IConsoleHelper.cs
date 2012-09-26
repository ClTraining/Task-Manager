using EntitiesLibrary;

namespace TaskConsoleClient.UI
{
    public interface IConsoleHelper
    {
        void View(ContractTask task);

        ContractTask Parse(string text);
    }
}
