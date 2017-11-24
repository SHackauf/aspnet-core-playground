using System.ComponentModel.DataAnnotations;

namespace de.playground.aspnet.core.contracts.pocos
{
    public interface ICustomerPoco : IPoco
    {
        [Key]
        int Id { get; set; }

        [Required]
        [StringLength(50)]
        string Name { get; set; }
    }
}
