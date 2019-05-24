using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class AgentReportModel
    {
        public string Username { get; set; }

        public Kaedcuser User { get; set; }

        public string TotalAmount { get; set; }
    }
}
