﻿using System.ComponentModel.DataAnnotations;

namespace de.playground.aspnet.core.contracts.pocos
{
    public class ProductPoco : IPoco
    {
        #region Public Properties

        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        #endregion
    }
}
