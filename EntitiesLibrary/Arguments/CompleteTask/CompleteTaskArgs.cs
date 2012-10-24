using System.ComponentModel;

namespace EntitiesLibrary.Arguments.CompleteTask
{
    [TypeConverter(typeof (CompleteTaskArgsConverter))]
    public class CompleteTaskArgs
    {
        public int Id { get; set; }
    }
}