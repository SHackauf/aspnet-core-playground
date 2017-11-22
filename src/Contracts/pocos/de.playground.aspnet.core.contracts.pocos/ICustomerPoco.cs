namespace de.playground.aspnet.core.contracts.pocos
{
    public interface ICustomerPoco : IPoco
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
