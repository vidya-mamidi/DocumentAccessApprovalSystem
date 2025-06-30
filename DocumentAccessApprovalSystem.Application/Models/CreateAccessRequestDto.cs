using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Domain.Entities;

namespace DocumentAccessApprovalSystem.Application.Models
{
    public class CreateAccessRequestDto
    {
        public  int  UserId { get; set; }
        public int DocumentId { get; set; }
        public AccessType RequestedAccess { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
