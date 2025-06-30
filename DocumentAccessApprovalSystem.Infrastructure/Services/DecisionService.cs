using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Application.Models;
using DocumentAccessApprovalSystem.Domain.Entities;
using DocumentAccessApprovalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace DocumentAccessApprovalSystem.Infrastructure.Services
{
    public class DecisionService : IDecisionService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
      
        public DecisionService(AppDbContext appDbContext, IEmailService emailService, INotificationService notificationService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
            _notificationService = notificationService;
        }
        public async Task MakeDecision(DecisionDto decisionDto)
        {
            var duplicateExists = await _appDbContext.AccessRequests.AnyAsync(r => r.Id == decisionDto.AccessRequestId);
         
            if (duplicateExists)
                throw new InvalidOperationException("Same Request was already Done.");

            var request = await _appDbContext.AccessRequests.FindAsync(decisionDto.AccessRequestId);
            if (request == null)
                throw new Exception($"AccessRequest with ID {decisionDto.AccessRequestId} was not found.");

            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be decided.");

            request.Status = decisionDto.Approved ? RequestStatus.Approved : RequestStatus.Rejected;
            request.Decision = new Decision
            {
                AccessRequestId = decisionDto.AccessRequestId,
                Approved = decisionDto.Approved,
                Comment = decisionDto.Comment
            };
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("The access request was modified or deleted by another process.");
            }

            _notificationService.Enqueue(new Notification
            {
                RequestId = request.Id,
                Message = $"Access request has been {(decisionDto.Approved ? "approved" : "rejected")}"
            });


            _emailService.Enqueue(new Email
            {
                To = "mamidividya97@gmail.com",
                Subject = "Access Request Decision",
                Body = $"your access request has been {(decisionDto.Approved ? "approved" : "rejected")}\nComment : {decisionDto.Comment}"
            });
        }
        public async Task<IEnumerable<AccessRequest>> GetPendingRequests()
        {
            var result = await _appDbContext.AccessRequests.Where(x => x.Status == RequestStatus.Pending)
                  .Include(x => x.User)
                  .Include(x => x.Document)
                  .ToListAsync();
          
            Console.WriteLine($"All Requests count: {result.Count}");
            foreach (var r in result)
            {
                Console.WriteLine($"Id: {r.Id}, Status: {r.Status}");
            }
            return result;
        }

        public async Task<IEnumerable<AccessRequest>> GetRequests()
        {
            return await _appDbContext.AccessRequests
                 .Include(x => x.User)
                 .Include(x => x.Document)
                 .ToListAsync();
        }
    }
}
