using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAccessApprovalSystem.Application.Models
{
    public class DecisionDto
    {
        public int AccessRequestId { get; set; }
        public bool Approved { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
