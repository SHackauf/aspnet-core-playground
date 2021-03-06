﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace de.playground.aspnet.core.mvc.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }

        [Display(Name = "Customer name")]
        [StringLength(60, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
    }
}
