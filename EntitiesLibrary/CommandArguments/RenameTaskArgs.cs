namespace EntitiesLibrary.CommandArguments
{
    public interface IRenameTaskArgs : ICommandArguments
    {
        string Name { get; set; }
    }

    public class RenameTaskArgs : IRenameTaskArgs
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}