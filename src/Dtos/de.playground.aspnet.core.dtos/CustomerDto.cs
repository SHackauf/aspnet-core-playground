using de.playground.aspnet.core.contracts.dtos;

namespace de.playground.aspnet.core.dtos
{
    public class CustomerDto : ICustomerDto
    {
        #region Public Properties

        public int Id { get; set; }

        public string Name { get; set; }

        #endregion
    }
}
