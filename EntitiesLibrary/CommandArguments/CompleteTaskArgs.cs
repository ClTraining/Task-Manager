namespace EntitiesLibrary.CommandArguments
{

    public class CompleteTaskArgs : IEditCommandArguments
    {
        private bool isCompleted;

        public int Id { get; set; }
    }
}