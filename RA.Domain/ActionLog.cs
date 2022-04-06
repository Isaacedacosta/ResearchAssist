using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Domain
{
    public class ActionLog
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string Register { get; set; }
    }
}
