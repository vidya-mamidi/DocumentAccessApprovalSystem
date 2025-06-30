using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Domain.Entities;

namespace DocumentAccessApprovalSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly Queue<Email> _queue = new();
        private readonly object _lock = new();

        public void Enqueue(Email msg)
        {
            lock (_lock)
            {
                _queue.Enqueue(msg);
            }
        }

        public bool TryDequeue(out Email msg)
        {
            lock (_lock)
            {
                if (_queue.Count > 0)
                {
                    msg = _queue.Dequeue();
                    return true;
                }

                msg = null!;
                return false;
            }
        }
    }
}
