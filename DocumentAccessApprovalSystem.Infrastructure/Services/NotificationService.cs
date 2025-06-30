using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Domain.Entities;

namespace DocumentAccessApprovalSystem.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly Queue<Notification> _messages = new();
        private readonly object _lock = new();
        public void Enqueue(Notification message)
        {
            lock (_lock) { _messages.Enqueue(message); }
        }

        public bool TryDequeue(out Notification message)
        {
            lock (_lock)
            {
                if (_messages.Count > 0)
                {
                    message = _messages.Dequeue();
                    return true;
                }

                message = null!;
                return false;
            }
        }
    }
}
