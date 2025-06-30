using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Domain.Entities;

namespace DocumentAccessApprovalSystem.Application.Interfaces
{
    public interface INotificationService
    {
        void Enqueue(Notification message);
        bool TryDequeue(out Notification message);
    }
}
