using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TTDataAccessLibrary.Models
{
    public class Asset
    {
        [Required]
        public int AssetId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int TypeBitField { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
