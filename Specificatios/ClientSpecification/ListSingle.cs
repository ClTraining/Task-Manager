namespace Specifications.ClientSpecification
{
    public class ListSingle : IClientSpecification
    {
        public int ID { get; private set; }

        public ListSingle(int id)
        {
            ID = id;
        }
    }
}
