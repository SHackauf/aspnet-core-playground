namespace de.playground.aspnet.core.contracts.pocos
{
    public interface IProductPoco : IPoco
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        string Name { get; set; }
    }
}
