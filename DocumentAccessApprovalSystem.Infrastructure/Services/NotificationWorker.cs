using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DocumentAccessApprovalSystem.Application.Interfaces;

namespace DocumentAccessApprovalSystem.Infrastructure.Services
{
    public class NotificationWorker : BackgroundService
    {
        private readonly ILogger<NotificationWorker> _logger;
        private readonly INotificationService _notificationService;

        public NotificationWorker(ILogger<NotificationWorker> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_notificationService.TryDequeue(out var message))
                {
                    var text = $"🔔 Notification: {message.Message} [RequestId: {message.RequestId}]";
                    FileLogger.Log("notifications.log", text);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
