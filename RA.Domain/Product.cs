using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Domain
{
    public class Product
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Manufacturer { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
}
