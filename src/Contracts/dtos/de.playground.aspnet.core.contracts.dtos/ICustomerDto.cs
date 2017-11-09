namespace de.playground.aspnet.core.contracts.dtos
{
    public interface ICustomerDto : IDto
    {
        int Id { get;  set; }
        string Name { get;  set; }
    }
}