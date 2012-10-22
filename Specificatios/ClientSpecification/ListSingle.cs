namespace Specifications.ClientSpecification
{
    public class ListSingle : IClientSpecification
    {
        public ListSingle(int id)
        {
            Data = id;
        }

        public object Data { get; set; }
    }
}
