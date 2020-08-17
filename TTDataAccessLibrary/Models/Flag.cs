using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TTDataAccessLibrary.Models
{
    public class Flag
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int BitFlag { get; set; }
    }
}
