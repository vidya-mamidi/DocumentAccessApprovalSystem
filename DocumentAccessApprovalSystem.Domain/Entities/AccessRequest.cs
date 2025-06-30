using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAccessApprovalSystem.Domain.Entities
{
    public class AccessRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int DocumentId { get; set; }
        public Document? Document { get; set; }
        public AccessType RequestedAccess { get; set; }
        public string Reason { get; set; } = string.Empty;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public Decision? Decision { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
