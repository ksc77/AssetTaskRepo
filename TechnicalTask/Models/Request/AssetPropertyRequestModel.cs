using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalTask.Models.Request
{
    public class AssetPropertyRequestModel
    {
        public int AssetId { get; set; }
        public string Property { get; set; }
        public bool PropertyValue { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
