using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DocumentAccessApprovalSystem.Infrastructure.Services
{
    public class EmailWorker : BackgroundService
    {
        private readonly ILogger<EmailWorker> _logger;
        private readonly IEmailService _emailService;

        public EmailWorker(ILogger<EmailWorker> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email worker started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_emailService.TryDequeue(out var email))
                {

                    var text = $"📧 Email to {email.To}\nSubject: {email.Subject}\nBody: {email.Body}";
                    FileLogger.Log("emails.log", text);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
