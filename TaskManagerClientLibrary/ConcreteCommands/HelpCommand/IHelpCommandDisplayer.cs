namespace TaskManagerClientLibrary.ConcreteCommands.HelpCommand
{
    public interface IHelpCommandDisplayer
    {
        void Show(ICommand command);
    }
}