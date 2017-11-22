using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.pocos
{
    public class ProductPoco : IProductPoco
    {
        #region Public Properties

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Name { get; set; }

        #endregion
    }
}
