namespace EntitiesLibrary.CommandArguments
{

    public class RenameTaskArgs : IEditCommandArguments
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}