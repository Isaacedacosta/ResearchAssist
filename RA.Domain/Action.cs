using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Domain
{
    public class Action
    {
        public int? Id { get; set; }
        public string ActionName { get; set; }
        public Employer Employer { get; set; }
        public DateTime Date { get; set; }
        public Subject Subject { get; set; }
        public Product? Product { get; set; }
        public string? ActionLog { get; set; }

    }
}
