namespace de.playground.aspnet.core.contracts.dtos
{
    public interface IProductDto : IDto
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        string Name { get; set; }
    }
}
