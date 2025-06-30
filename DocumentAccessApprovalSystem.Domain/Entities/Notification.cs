using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAccessApprovalSystem.Domain.Entities
{
    public class Notification
    {
        public int RequestId { get; set; }
        public string Message { get; set; }
    }
}
