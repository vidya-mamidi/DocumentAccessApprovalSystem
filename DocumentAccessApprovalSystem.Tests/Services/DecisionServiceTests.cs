using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DocumentAccessApprovalSystem.Application.Models;
using DocumentAccessApprovalSystem.Domain.Entities;
using DocumentAccessApprovalSystem.Infrastructure.Services;
using DocumentAccessApprovalSystem.Tests.TestUtilities;
using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentAccessApprovalSystem.Tests.Services
{
    public class DecisionServiceTests
    {

        private readonly AppDbContext _appDbContext;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly DecisionService _decisionService;

        public DecisionServiceTests()
        {

            _appDbContext = InMemoryDbContextFactory.Create("TestDb");
            _emailServiceMock = new Mock<IEmailService>();
            _notificationServiceMock = new Mock<INotificationService>();

            _decisionService = new DecisionService(
                _appDbContext,
                _emailServiceMock.Object,
                _notificationServiceMock.Object
            );
        }


        [Fact]
        public async Task MakeDecision_ApprovesRequest_UpdatesStatusAndDecision()
        {

            var request = new AccessRequest
            {
                Id = 1,
                UserId = 1,
                DocumentId = 7,
                RequestedAccess = AccessType.Read,
                Reason = "Need access",
                Status = RequestStatus.Pending
            };

            var duplicateExists = await _appDbContext.AccessRequests.AnyAsync(r =>
            r.Id == request.Id ||
          r.UserId == request.UserId &&
          r.DocumentId == request.DocumentId);

            if (duplicateExists)
                throw new InvalidOperationException("You have already submitted a  request for this document.");

            _appDbContext.AccessRequests.Add(request);


           

            await _appDbContext.SaveChangesAsync();

            var dto = new DecisionDto
            {
                AccessRequestId = 1,
                Approved = true,
                Comment = "Looks good"
            };

            // Act
            await _decisionService.MakeDecision(dto);

            // Assert
            var updated = await _appDbContext.AccessRequests.FindAsync(1);
            Assert.Equal(RequestStatus.Approved, updated.Status);
            Assert.NotNull(updated.Decision);
            Assert.True(updated.Decision.Approved);
            Assert.Equal("Looks good", updated.Decision.Comment);
        }

        [Fact]
        public async Task MakeDecision_RequestNotFound_Throws()
        {
           

            var dto = new DecisionDto
            {
                AccessRequestId = 999,
                Approved = false,
                Comment = "Invalid"
            };

            var ex = await Assert.ThrowsAsync<System.Exception>(() => _decisionService.MakeDecision(dto));
            Assert.Equal($"AccessRequest with ID {dto.AccessRequestId} was not found.", ex.Message);
        }

        [Fact]
        public async Task GetPendingRequestsAsync_ReturnsOnlyPending()
        {

            _appDbContext.Users.AddRange(
                new User { Id = 1, Name = "Vidya" },
                new User { Id = 2, Name = "Varun" }
            );

            //seed Documents incrementally 
            var documentTitles = new[] { "Employee Handbook", "Onboarding Checklist", "User Guide", "Data Privacy Policy", "Change Request Form", "Version Control Guidelines", "Security Policy", "Incident Report Template", "Data Privacy Policy" };

            foreach (var title in documentTitles)
            {
               
                    _appDbContext.Documents.AddRange(new Document { Title = title });
             
            }

            _appDbContext.AccessRequests.AddRange(
                new AccessRequest { Id = 1, UserId = 1 , DocumentId = 1, RequestedAccess = AccessType.Read, Status = RequestStatus.Pending },
                new AccessRequest { Id = 2, UserId = 2, DocumentId = 2, RequestedAccess = AccessType.Edit, Status = RequestStatus.Approved }
            );
            await _appDbContext.SaveChangesAsync();


            var allRequests = await _appDbContext.AccessRequests.ToListAsync();
            Console.WriteLine($"All Requests count: {allRequests.Count}");
            foreach (var r in allRequests)
            {
                Console.WriteLine($"Id: {r.Id}, Status: {r.Status}");
            }

            var pending = await _decisionService.GetPendingRequests();

            Assert.Single(pending);
            Assert.Equal(1, pending.First().Id);
        }
    }
}
