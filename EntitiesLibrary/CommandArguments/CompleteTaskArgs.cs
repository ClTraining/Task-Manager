namespace EntitiesLibrary.CommandArguments
{
    public interface ICompleteTaskArgs : ICommandArguments
    {
    }

    public class CompleteTaskArgs : ICompleteTaskArgs
    {
        private bool isCompleted;

        public int Id { get; set; }
    }
}