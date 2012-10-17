namespace TaskManagerClientLibrary.ConcreteHandlers.HelpCommand
{
    public interface IHelpDisplayer
    {
        void Show(ICommand command);
    }
}